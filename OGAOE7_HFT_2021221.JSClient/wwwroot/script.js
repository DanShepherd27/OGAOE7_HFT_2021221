let pizzas = [];

getdata();

async function getdata() {
    await fetch('http://localhost:26548/pizza')
        .then(x => x.json())
        .then(y => {
            pizzas = y;
            console.log(pizzas);
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
                <td>
                    <button type="button" onclick="remove(${t.id})">Delete</button>
                </td>
            </tr>`;
    });
}

function remove(id) {
    alert(id);
}

function create() {
    let name = document.getElementById('pizzaname').value;
    let price = document.getElementById('pizzaprice').value;
    fetch('http://localhost:26548/pizza', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify({ name: name, price: price}),
    })
        .then(response => response)
        .then(data => { console.log('Success:', data); getdata();})
        .catch(error => { console.error('Error:', error); });
    getdata();
}