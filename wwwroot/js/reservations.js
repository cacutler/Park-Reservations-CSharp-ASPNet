const API = '/api';
async function loadMyReservations() {
    const res = await fetch(`${API}/reservations/mine`, {
        headers: authHeaders()
    });
    if (!res.ok) throw new Error('Failed to load reservations');
    return res.json();
}
async function createReservation(data) {
    const res = await fetch(`${API}/reservations`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}
async function updateReservation(reservationId, data) {
    const res = await fetch(`${API}/reservations/${reservationId}`, {
        method: 'PUT',
        headers: authHeaders(),
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}
async function deleteReservation(reservationId) {
    const res = await fetch(`${API}/reservations/${reservationId}`, {
        method: 'DELETE',
        headers: authHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete reservation');
}
function renderReservations(reservations, currentContainerId, pastContainerId) {
    const today = new Date().toISOString().split('T')[0];
    const current = reservations.filter(r => r.date >= today);
    const past = reservations.filter(r => r.date < today);
    renderReservationList(current, currentContainerId, true);
    renderReservationList(past, pastContainerId, false);
}

function renderReservationList(reservations, containerId, allowActions) {
    const container = document.getElementById(containerId);
    if (!reservations.length) {
        container.innerHTML = '<p>No reservations found.</p>';
        return;
    }
    container.innerHTML = reservations.map(r => `
        <div class="reservation-card" data-id="${r.id}">
            <h3>${r.parkName} — ${r.cityName}</h3>
            <p>Date: ${r.date}</p>
            <p>Time: ${r.time}</p>
            <p>Name: ${r.name}</p>
            <p>Email: ${r.email}</p>
            <p>Phone: ${r.phoneNumber}</p>
            <p>Status: <span class="status status-${r.status.toLowerCase()}">${r.status}</span></p>
            ${allowActions && r.status === 'Pending' ? `
                <button onclick="editReservation(${r.id})">Edit</button>
                <button onclick="confirmDelete(${r.id})">Cancel</button>
            ` : ''}
        </div>
    `).join('');
}
function editReservation(reservationId) {
    window.location.href = `/pages/edit-reservation.html?reservationId=${reservationId}`;
}
async function confirmDelete(reservationId) {
    if (!confirm('Are you sure you want to cancel this reservation?')) return;
    try {
        await deleteReservation(reservationId);
        alert('Reservation cancelled.');
        window.location.reload();
    } catch (err) {
        alert(err.message);
    }
}
function buildReservationPayload(parkId) {
    return {
        parkId: parseInt(parkId),
        date: document.getElementById('date').value,
        time: document.getElementById('time').value,
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phone').value
    };
}