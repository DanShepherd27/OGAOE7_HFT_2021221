let pizzas = [];
let connection = null;
let pizzaIdToUpdate = -1;

getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:26548/hub")
        .configureLogging(signalR.LogLevel.information)
        .build();

    connection.on("PizzaCreated", (user, message) => {
        //console.log(user);
        //console.log(message);
        getdata();
    });

    connection.on("PizzaDeleted", (user, message) => {
        //console.log(user);
        //console.log(message);
        getdata();
    });

    connection.on("PizzaUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}

async function getdata() {
    await fetch('http://localhost:26548/pizza')
        .then(x => x.json())
        .then(y => {
            pizzas = y;
            //console.log(pizzas);
            display();
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = '';
    pizzas.forEach(t => {
        console.log(`${t.name} ${t.price} ${t.promotional}`);
        document.getElementById('resultarea').innerHTML +=
            `<tr> 
                <td> ${t.id} </td> 
                <td> ${t.name} </td>
                <td> ${t.price} </td>
                <td> ${t.promotional} </td>
                <td id="buttoncolumn">
                    <button type="button" class="deletebutton" onclick="remove(${t.id})">Delete</button>
                    <button type="button" class="updatebutton" onclick="showupdate(${t.id}); hideadd();">Update</button>
                </td>
            </tr>`;
    });
}

function create() {
    let name = document.getElementById('pizzaname').value;
    let price = document.getElementById('pizzaprice').value;
    fetch('http://localhost:26548/pizza', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify({ name: name, price: price }),
    })
        .then(response => response)
        .then(data => { console.log('Success: ', data); getdata(); })
        .catch((error) => { console.error('Error: ', error); });
    getdata();
    document.getElementById('pizzaname').value = '';
    document.getElementById('pizzaprice').value = '';
}

function remove(id) {
    fetch('http://localhost:26548/pizza/id/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success: ' + data);
            getdata();
        })
        .catch((error) => { console.error('Error: ', error); })
}

function update() {
    let name = document.getElementById('pizzanametoupdate').value;
    let price = document.getElementById('pizzapricetoupdate').value;
    let pizza = { id: pizzaIdToUpdate, name: name, price: price };
    console.log(pizza);
    fetch('http://localhost:26548/pizza', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify({ id: pizzaIdToUpdate, name: name, price: price }),
    })
        .then(response => response)
        .then(data => {
            console.log('Success: ', data);
            getdata();
        })
        .catch((error) => { console.error('Error: ', error); });
    hideupdate();
    showadd();
}

function showadd() {
    document.getElementById('formdiv').style.display = 'flex';
}

function hideadd() {
    document.getElementById('formdiv').style.display = 'none';
}

function showupdate(id) {
    document.getElementById('pizzanametoupdate').value = pizzas.find(t => t['id'] == id)['name'];
    document.getElementById('pizzapricetoupdate').value = pizzas.find(t => t['id'] == id)['price'];
    document.getElementById('updateformdiv').style.display = 'flex';
    pizzaIdToUpdate = id;
}

function hideupdate() {
    document.getElementById('updateformdiv').style.display = 'none';
}