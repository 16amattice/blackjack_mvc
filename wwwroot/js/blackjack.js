var getGameStateUrl;
document.addEventListener("DOMContentLoaded", function () {
    getGameStateUrl = document.getElementById("getGameStateUrl").value;
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
    fetch(getGameStateUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(gameState => {
            if (gameState && gameState.PlayerHand.length === 0 && gameState.DealerHand.length === 0) {
                sendAction("init");
            } else {
                updateGameState(gameState);
            }
        })
        .catch(error => {
            console.error("There was a problem with the fetch operation: ", error);
        });
}
function sendAction(action) {
    const gameStatus = document.getElementById("results").innerText;
    if (gameStatus) {
        return;
    }
    const xhr = new XMLHttpRequest();
    xhr.open("POST", "/Blackjack/" + action, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const updatedModel = JSON.parse(xhr.responseText);
            updateGameState(updatedModel);
        } else if (xhr.readyState == 4) {
            console.error("Error:", xhr.status, xhr.statusText, xhr.responseText);
        }
    };
    xhr.send();
}
function updateGameState(gameState) {
    updateCards("your-cards", gameState.playerHand);
    updateCards("dealer-cards", gameState.dealerHand);
    if (gameState.gameStatus) {
        document.getElementById("results").innerText = gameState.gameStatus;
        document.getElementById("hit").disabled = true;
        document.getElementById("stay").disabled = true;
        document.getElementById("restart").style.visibility = "visible";
    }
}
function updateCards(containerId, hand) {
    if (!hand) return;
    const container = document.getElementById(containerId);
    container.innerHTML = '';
    hand.forEach(card => {
        const cardImg = document.createElement("img");
        const cardFileName = card.isHidden ? "back" : card.value + '-' + card.suit;
        cardImg.src = "/images/cards/" + cardFileName + ".png";
        container.appendChild(cardImg);
    });
}