function togglePW() {
    const password = document.querySelector('[name=password]');
    if (password.getAttribute("type") === "password") {
        password.setAttribute("type", "text");
        document.getElementById("font").style.color = 'black';
    } else {
        password.setAttribute('type', 'password');
        document.getElementById("font").style.color = 'white';
    }
}

function togglePW_2() {
    const password = document.querySelector('[name=password_2]');
    if (password.getAttribute("type") === "password") {
        password.setAttribute("type", "text");
        document.getElementById("font_2").style.color = 'black';
    } else {
        password.setAttribute('type', 'password');
        document.getElementById("font_2").style.color = 'white';
    }
}
