document.addEventListener("DOMContentLoaded", function () {
    fetchGameState();

    document.getElementById("hit").addEventListener("click", function () {
        sendAction("hit");
    });

    document.getElementById("stay").addEventListener("click", function () {
        sendAction("stay");
    });

    document.getElementById("restart").addEventListener("click", function () {
        location.reload();
    });
});

function fetchGameState() {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", "/Blackjack/GetGameState", true);

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const updatedModel = JSON.parse(xhr.responseText);
            updateView(updatedModel);
        }
    };

    xhr.send();
}

function sendAction(action) {
    const xhr = new XMLHttpRequest();
    xhr.open("POST", "/Blackjack/" + action, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const updatedModel = JSON.parse(xhr.responseText);
            updateView(updatedModel);
        }
    };

    xhr.send();
}

function updateView(model) {
    updateCards("your-cards", model.PlayerHand);
    updateCards("dealer-cards", model.DealerHand);
    document.getElementById("your-sum").innerText = model.PlayerSum;
    document.getElementById("dealer-sum").innerText = model.DealerSum;
    document.getElementById("results").innerText = model.GameStatus;
}

function updateCards(containerId, hand) {
    const container = document.getElementById(containerId);
    container.innerHTML = ''; // Clear previous cards

    hand.forEach(card => {
        const cardImg = document.createElement("img");
        const cardFileName = card.IsHidden ? "back" : card.Value + card.Suit;
        cardImg.src = "../images/cards/" + cardFileName + ".png";
        container.appendChild(cardImg);
    });
}
