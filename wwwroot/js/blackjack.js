var getGameStateUrl;

document.addEventListener("DOMContentLoaded", function () {
    // Set the value of getGameStateUrl
    
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

// Update the fetchGameState() function
function fetchGameState() {
    fetch(getGameStateUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(gameState => {
            console.log("Response received: ", gameState);

            if (gameState && gameState.PlayerHand.length === 0 && gameState.DealerHand.length === 0) {
                console.log("Game state is not initialized, initializing now.");
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
    const xhr = new XMLHttpRequest();
    xhr.open("POST", "/Blackjack/" + action, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const updatedModel = JSON.parse(xhr.responseText);
            console.log("Received updated game state:", updatedModel);
            updateGameState(updatedModel); // Use PascalCase property names
        } else if (xhr.readyState == 4) {
            console.error("Error:", xhr.status, xhr.statusText, xhr.responseText);
        }
    };

    xhr.send();
}






function updateGameState(gameState) {
    console.log("Updating game state with:", gameState);
    updateCards("your-cards", gameState.PlayerHand);
    updateCards("dealer-cards", gameState.DealerHand);

    if (gameState.playerSum) {
        document.getElementById("your-sum").innerText = gameState.playerSum;
    }

    if (gameState.dealerSum) {
        document.getElementById("dealer-sum").innerText = gameState.dealerSum;
    }
    document.getElementById("results").innerText = gameState.gameStatus;
}






function updateCards(containerId, hand) {
    if (!hand) return; // Add this line to check if hand is null or undefined

    const container = document.getElementById(containerId);
    container.innerHTML = ''; // Clear previous cards

    hand.forEach(card => {
        const cardImg = document.createElement("img");
        const cardFileName = card.IsHidden ? "back" : card.Value + card.Suit;
        cardImg.src = "../images/cards/" + cardFileName + ".png";
        container.appendChild(cardImg);
    });
}

