document.addEventListener('DOMContentLoaded', function () {
    const datePickerInput = document.getElementById('dateOfBirth');
    const calendarIcon = document.querySelector('.calendar-icon');

    calendarIcon.addEventListener('click', function () {
        datePickerInput.focus(); // Focus sur le champ de date
    });
});
