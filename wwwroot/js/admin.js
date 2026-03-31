const API = '/api';
async function loadPendingReservations() {
    const res = await fetch(`${API}/reservations/pending`, {
        headers: authHeaders()
    });
    if (!res.ok) throw new Error('Failed to load pending reservations');
    return res.json();
}
async function updateReservationStatus(reservationId, status) {
    const res = await fetch(`${API}/reservations/${reservationId}/status`, {
        method: 'PATCH',
        headers: authHeaders(),
        body: JSON.stringify(status)
    });
    if (!res.ok) {
        const err = await res.text();
        throw new Error(err || 'Failed to update status');
    }
    return res.json();
}
async function grantAdminAccess(userId) {
    const res = await fetch(`${API}/admin/grant`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify({ userId })
    });
    if (!res.ok) throw new Error(await res.text());
    return res.text();
}
async function revokeAdminAccess(userId) {
    const res = await fetch(`${API}/admin/revoke/${userId}`, {
        method: 'DELETE',
        headers: authHeaders()
    });
    if (!res.ok) throw new Error(await res.text());
    return res.text();
}
async function loadAdminParks() {
    const res = await fetch(`${API}/parks`, { headers: authHeaders() });
    return res.json();
}
async function renderPendingReservations(containerId) {
    const container = document.getElementById(containerId);
    try {
        const reservations = await loadPendingReservations();
        if (!reservations.length) {
            container.innerHTML = '<p>No pending reservations.</p>';
            return;
        }
        container.innerHTML = reservations.map(r => `
            <div class="reservation-card pending" data-id="${r.id}">
                <h3>${r.parkName}</h3>
                <p>Requested by: ${r.name} (${r.email})</p>
                <p>Date: ${r.date} at ${r.time}</p>
                <p>Phone: ${r.phoneNumber}</p>
                <div class="actions">
                    <button class="approve-btn" onclick="handleStatus(${r.id}, 'Approved')">Approve</button>
                    <button class="deny-btn" onclick="handleStatus(${r.id}, 'Denied')">Deny</button>
                </div>
            </div>
        `).join('');
    } catch (err) {
        container.innerHTML = `<p>Error: ${err.message}</p>`;
    }
}
async function handleStatus(reservationId, status) {
    try {
        await updateReservationStatus(reservationId, status);
        alert(`Reservation ${status.toLowerCase()} successfully.`);
        await renderPendingReservations('pending-container');
    } catch (err) {
        alert(err.message);
    }
}
async function renderAdminParks(containerId) {
    const container = document.getElementById(containerId);
    try {
        const parks = await loadAdminParks();
        if (!parks.length) {
            container.innerHTML = '<p>No parks yet. Create one below.</p>';
            return;
        }
        container.innerHTML = parks.map(p => `
            <div class="park-card" data-id="${p.id}">
                <h3>${p.name}</h3>
                <p>${p.address}</p>
                <button onclick="editPark(${p.id})">Edit</button>
            </div>
        `).join('');
    } catch (err) {
        container.innerHTML = `<p>Error: ${err.message}</p>`;
    }
}
function editPark(parkId) {
    window.location.href = `/pages/edit-park.html?parkId=${parkId}`;
}
function buildScheduleFromForm() {
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
    const schedule = {};
    days.forEach(day => {
        const input = document.getElementById(`schedule-${day.toLowerCase()}`);
        if (input && input.value.trim()) {
            schedule[day] = input.value.split(',').map(s => s.trim()).filter(Boolean);
        }
    });
    return JSON.stringify(schedule);
}