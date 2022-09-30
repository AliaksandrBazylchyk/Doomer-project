function Login(){
    let url = 'https://localhost:7174/api/auth/login'
    let login = document.querySelector([name="searchTxt"]).value
    let password = document.querySelector([name="searchTxt"]).value
    let bodyData = {
        'login':login,
        'password':pass
    }
    let data = {
        headers:{
            "content-type":"application/json; charset=UTF-8", 
            "access-control-allow-origin":"*"
        },
        body: bodyData,
        method: "POST"
    }
    fetch(url,data).then(
        response =>{
            Console.log(response.json())
        }
    )
}
function Register(){
    let url = 'https://localhost:7174/api/auth/register'
}

function Unsecured(){
    let url = 'https://localhost:7174/api/auth/unsecured/login'
}