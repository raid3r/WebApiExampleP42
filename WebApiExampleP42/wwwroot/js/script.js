
const userListContainer = document.getElementById('users');


async function fetchUsers() { 
    // todo
    const response = await fetch('/api/user');

    const users = await response.json();

    const table = document.createElement('table');
    const headerRow = document.createElement('tr');
    headerRow.innerHTML = '<th>ID</th><th>Name</th><th>Email</th>';
    table.appendChild(headerRow);

    const tbody = document.createElement('tbody');
    users.forEach(user => {
        const row = document.createElement('tr');
        row.innerHTML = `<td>${user.id}</td><td>${user.name}</td><td>${user.email}</td>`;
        tbody.appendChild(row);
    });
    table.appendChild(tbody);
    userListContainer.appendChild(table);
}



document.addEventListener('DOMContentLoaded', () => {
    fetchUsers();
});
