const API = '/api';
async function loadCities() {
    const res = await fetch(`${API}/cities`);
    return res.json();
}
async function loadParks(cityId = null) {
    const url = cityId ? `${API}/parks?cityId=${cityId}` : `${API}/parks`;
    const res = await fetch(url);
    return res.json();
}
async function loadPark(parkId) {
    const res = await fetch(`${API}/parks/${parkId}`);
    return res.json();
}
async function createPark(parkData) {
    const res = await fetch(`${API}/parks`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify(parkData)
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}
async function updatePark(parkId, parkData) {
    const res = await fetch(`${API}/parks/${parkId}`, {
        method: 'PUT',
        headers: authHeaders(),
        body: JSON.stringify(parkData)
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}
function renderParks(parks, containerId) {
    const container = document.getElementById(containerId);
    if (!parks.length) {
        container.innerHTML = '<p>No parks found.</p>';
        return;
    }
    container.innerHTML = parks.map(park => `
        <div class="park-card" data-id="${park.id}">
            <h3>${park.name}</h3>
            <p>${park.address}</p>
            <p>${park.cityName}</p>
            <button onclick="viewPark(${park.id})">View Details</button>
        </div>
    `).join('');
}
function renderSchedule(schedule, containerId) {
    const container = document.getElementById(containerId);
    let parsed;
    try {
        parsed = typeof schedule === 'string' ? JSON.parse(schedule) : schedule;
    } catch {
        container.innerHTML = '<p>No schedule available.</p>';
        return;
    }
    const days = Object.entries(parsed);
    if (!days.length) {
        container.innerHTML = '<p>No schedule available.</p>';
        return;
    }
    container.innerHTML = `
        <table>
            <thead><tr><th>Day</th><th>Available Times</th></tr></thead>
            <tbody>
                ${days.map(([day, times]) => `
                    <tr>
                        <td>${day}</td>
                        <td>${times.join(', ')}</td>
                    </tr>
                `).join('')}
            </tbody>
        </table>
    `;
}
function viewPark(parkId) {
    window.location.href = `/pages/park-detail.html?parkId=${parkId}`;
}