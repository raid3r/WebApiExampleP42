


const registerForm = document.getElementById('registerForm');



registerForm.addEventListener('submit', async (event) => {
    event.preventDefault();

    const formData = new FormData(registerForm);

    const registerRequest = {
        name: formData.get('name'),
        email: formData.get('email'),
        password: formData.get('password')
    }

    const errorMessageElement = document.getElementById('register-errors');
    fetch('/api/User/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerRequest)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            errorMessageElement.textContent = 'Registration successful!';
            //  Save the token in localStorage
            localStorage.setItem('token', data.token);
        } else {       

            /*
{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":
{"Name":["Name must be at least 3 characters long."],"Email":["Invalid email format."],"Password":["Password must be at least 6 characters long."]},"traceId":"00-47380a69b0ea876d3d2c1092effb9c80-78eed854a1fb873a-00"}
*/

            const errors = data.errors;

            const errorMessages = [];
            for (const key in errors) {
                if (errors.hasOwnProperty(key)) {
                    errorMessages.push(...errors[key]);
                }
            }

            errorMessageElement.textContent = errorMessages.join(', ');
        }
    })
    .catch(error => {
        errorMessageElement.textContent = 'An error occurred: ' + error.message;
    });



})


async function getAllMessages() {

        const token = localStorage.getItem('token');

    return await fetch('/api/Message', {
        method: 'GET',
        headers: {
            'X-My-authorization': token
        }
    }).then(response => response.json())
        .then(messages => {

            // Виводимо повідомлення на сторінку

        })

}

/*
Створити веб-сторінку з формою входу користувача.Вимоги:
Форма входу
Поля: логін (або email) та пароль.
Кнопка «Увійти».
При відправці форми дані надсилаються на сервер (використати fetch або XMLHttpRequest).
Отримати відповідь від сервера.

Обробка відповіді
Якщо логін і пароль вірні — сервер повертає токен.
Зберегти отриманий токен у localStorage.

Кнопка «Показати всі повідомлення»
При натисканні кнопки зробити запит на сервер з використанням збереженого токена (передати його в заголовку Authorization).
Отримати дані (список повідомлень).
Вивести отримані повідомлення на сторінці.

*/

