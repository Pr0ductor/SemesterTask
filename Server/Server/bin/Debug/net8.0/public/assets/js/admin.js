document.addEventListener('DOMContentLoaded', function () {
    // ���������� ��� ����� ���������� ������������
    document.getElementById('add-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                throw new Error('This user already in database');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.Login}</td>
                <td>${result.Password}</td>
                <td>${result.Email}</td>
                
            `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('userTableBody');
            table.appendChild(newRow);
            document.getElementById('addUserLogin').value = '';
            document.getElementById('addUserPassword').value = '';
            alert('User add succesfully !')
            

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });

    // ���������� ��� ����� ���������� ������
    document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                throw new Error('This movie already in database');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.id}</td>
                <td>${result.title}</td>
                <td>${result.release_year}</td>
                <td>${result.duration}</td>
                <td>${result.description}</td>
                <td>${result.poster_url}</td>
                <td>${result.director}</td>
                <td>${result.actors}</td>
                <td>${result.URL_video}</td>
                <td>${result.country}</td>
                <td>${result.genre}</td>
            `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('movieTableBody');
            table.appendChild(newRow);

            alert('Movie add succesfully !')

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� ������������
    document.getElementById('delete-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();
            if (result == false) {
                throw new Error('This user not exist');
            }
            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('Error. Incorrect ID');
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
            result.forEach(user => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${user.Id}</td>
                    <td>${user.Login}</td>
                    <td>${user.Password}</td>
                    <td>${user.Email}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });


    document.getElementById('delete-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            if (result == false) {
                throw new Error('This movie not exist');
            }

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('Error. Incorrect ID');
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
            result.forEach( movie => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movie.id}</td>
                    <td>${movie.title}</td>
                    <td>${movie.release_year}</td>
                    <td>${movie.duration}</td>
                    <td>${movie.description}</td>
                    <td>${movie.poster_url}</td>
                    <td>${movie.director}</td>
                    <td>${movie.actors}</td>
                    <td>${movie.URL_video}</td>
                    <td>${movie.country}</td>
                    <td>${movie.genre}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });

    document.getElementById('update-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();
            if (result == false) {
                throw new Error('This user not exist');
            }
            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Error. Incorrect ID');
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(user => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                <td>${user.Id}</td>
                <td>${user.Login}</td>
                <td>${user.Password}</td>
                <td>${user.Email}</td>
            `;
                tableBody.appendChild(newRow);

            });
            alert('User update succesfully !')

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });

    document.getElementById('update-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            if (result == false) {
                throw new Error('This movie not exist');
            }

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Error. Incorrect ID');
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(movie => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                <td>${movie.id}</td>
                <td>${movie.title}</td>
                <td>${movie.release_year}</td>
                <td>${movie.duration}</td>
                <td>${movie.description}</td>
                <td>${movie.poster_url}</td>
                <td>${movie.director}</td>
                <td>${movie.actors}</td>
                <td>${movie.URL_video}</td>
                <td>${movie.country}</td>
                <td>${movie.genre}</td>
            `;
                tableBody.appendChild(newRow);
            });
            alert('Movie update succesfully !')

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });
});
