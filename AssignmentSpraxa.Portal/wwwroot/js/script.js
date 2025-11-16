// Navigation functionality
document.addEventListener('DOMContentLoaded', function() {
    const pageTitle = document.getElementById('pageTitle');
    const menuToggle = document.getElementById('menuToggle');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.querySelector('.main-content');

    // Sidebar toggle
    menuToggle.addEventListener('click', function() {
        sidebar.classList.toggle('collapsed');
        mainContent.classList.toggle('expanded');
    });

    // Mobile sidebar toggle
    if (window.innerWidth <= 768) {
        menuToggle.addEventListener('click', function() {
            sidebar.classList.toggle('active');
        });
    }
});

// Modal functionality
function openModal(modalId) {
    const modal = document.getElementById(modalId);
    modal.classList.add('active');
    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    modal.classList.remove('active');
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
}

// Close modal when clicking outside
document.addEventListener('click', function(e) {
    if (e.target.classList.contains('modal')) {
        closeModal(e.target.id);
    }
});

// Search functionality
document.addEventListener('DOMContentLoaded', function() {
    const patientSearch = document.getElementById('patientSearch');
    const appointmentSearch = document.getElementById('appointmentSearch');
    
    if (patientSearch) {
        patientSearch.addEventListener('input', function() {
            filterTable('patientsTable', this.value);
        });
    }
    
    if (appointmentSearch) {
        appointmentSearch.addEventListener('input', function() {
            filterTable('appointmentsTable', this.value);
        });
    }
});

function filterTable(tableId, searchTerm) {
    const table = document.getElementById(tableId);
    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    
    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const cells = row.getElementsByTagName('td');
        let found = false;
        
        for (let j = 0; j < cells.length; j++) {
            const cell = cells[j];
            if (cell.textContent.toLowerCase().includes(searchTerm.toLowerCase())) {
                found = true;
                break;
            }
        }
        
        row.style.display = found ? '' : 'none';
    }
}

// Form submission handling
document.addEventListener('submit', function(e) {
    if (e.target.classList.contains('modal-form')) {
        e.preventDefault();
        
        // Get form data
        const formData = new FormData(e.target);
        const data = Object.fromEntries(formData);
        
        // Here you would typically send the data to your backend
        console.log('Form data:', data);
        
        // Show success message (you can customize this)
        alert('Data saved successfully!');
        
        // Close the modal
        const modal = e.target.closest('.modal');
        closeModal(modal.id);
        
        // Reset form
        e.target.reset();
    }
});

// Sample data manipulation functions
function addPatientToTable(patientData) {
    const table = document.getElementById('patientsTable').getElementsByTagName('tbody')[0];
    const newRow = table.insertRow();
    
    newRow.innerHTML = `
        <td>#P${String(table.rows.length).padStart(3, '0')}</td>
        <td>
            <div class="patient-cell">
                <img src="~/img/user-default.jpg" alt="Patient">
                <span>${patientData.firstName} ${patientData.lastName}</span>
            </div>
        </td>
        <td>${patientData.email}</td>
        <td>${patientData.phone}</td>
        <td>New Patient</td>
        <td><span class="status active">Active</span></td>
        <td>
            <div class="action-buttons">
                <button class="btn-icon" title="View"><i class="fas fa-eye"></i></button>
                <button class="btn-icon" title="Edit"><i class="fas fa-edit"></i></button>
                <button class="btn-icon delete" title="Delete"><i class="fas fa-trash"></i></button>
            </div>
        </td>
    `;
}

function addAppointmentToTable(appointmentData) {
    const table = document.getElementById('appointmentsTable').getElementsByTagName('tbody')[0];
    const newRow = table.insertRow();
    
    newRow.innerHTML = `
        <td>#A${String(table.rows.length).padStart(3, '0')}</td>
        <td>
            <div class="patient-cell">
                <img src="~/img/user-default.jpg" alt="Patient">
                <span>${appointmentData.patientSelect}</span>
            </div>
        </td>
        <td>${appointmentData.appointmentDate} - ${appointmentData.appointmentTime}</td>
        <td>${appointmentData.doctorSelect}</td>
        <td>${appointmentData.appointmentType}</td>
        <td><span class="status pending">Pending</span></td>
        <td>
            <div class="action-buttons">
                <button class="btn-icon" title="View"><i class="fas fa-eye"></i></button>
                <button class="btn-icon" title="Edit"><i class="fas fa-edit"></i></button>
                <button class="btn-icon delete" title="Cancel"><i class="fas fa-times"></i></button>
            </div>
        </td>
    `;
}

// Responsive handling
window.addEventListener('resize', function() {
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.querySelector('.main-content');
    
    if (window.innerWidth > 768) {
        sidebar.classList.remove('active');
        mainContent.style.marginLeft = sidebar.classList.contains('collapsed') ? '80px' : '280px';
    } else {
        mainContent.style.marginLeft = '0';
    }
});
