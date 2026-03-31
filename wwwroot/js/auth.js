const API = '/api';
const authHeaders = () => ({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}`
});
async function login(username, password) {
    const res = await fetch(`${API}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    });
    if (!res.ok) throw new Error('Login failed');
    const data = await res.json();
    localStorage.setItem('token', data.token);
    localStorage.setItem('isAdmin', data.isAdmin);
    return data;
}
function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('isAdmin');
    window.location.href = '/pages/index.html';
}
function isLoggedIn() {
    return !!localStorage.getItem('token');
}
function isAdmin() {
    return localStorage.getItem('isAdmin') === 'true';
}
function requireAuth() {
    if (!isLoggedIn()) {window.location.href = '/pages/index.html';}
}
function requireAdmin() {
    if (!isAdmin()) {window.location.href = '/pages/reservations.html';}
}