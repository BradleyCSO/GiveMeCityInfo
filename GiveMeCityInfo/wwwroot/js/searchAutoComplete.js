const baseURI = 'https://localhost:7192/api/v2/cities';

async function searchCities(query) {
    const response = await fetch(`${baseURI}/?searchQuery=${query}`);
    return response.json();
}

let timeoutId;
const citySearchInput = document.querySelector('input[type=text]');

citySearchInput.addEventListener('input', async function (event) {
    if (timeoutId) {
        clearTimeout(timeoutId);
    }

    removeOldItems(event);

    // Wrapped in a timeout to make API calls less frequent  
    timeoutId = setTimeout(async function () {
        // Query API based on inputted value
        const cities = await searchCities(event.target.value);

        // Create a list element for each city returned
        const suggestionsList = document.createElement('ul');
        suggestionsList.setAttribute('class', 'list-group');

        for (const city of cities) {
            const suggestionItem = document.createElement('a');
            suggestionItem.style.cursor = 'pointer';
            suggestionItem.classList.add('text-dark', 'page-item:hover', 'list-group-item', 'mt-2');
            suggestionItem.innerHTML = city.name;
            suggestionItem.setAttribute('href', `/City?cityId=${city.id}`);

            suggestionsList.appendChild(suggestionItem);
        }

        citySearchInput.parentNode.appendChild(suggestionsList);
    }, 1000);
});

document.addEventListener('click', function (event) {
    // For when the user clicks outside of the city search field
    if (!citySearchInput.contains(event.target) && !event.target.classList.contains('list-group-item')) {
        removeOldItems(event);
    }
});

function removeOldItems(event) {
    // If the inputted field has been emptied, clear out all old list items
    if (!event.target.value || event.target.value) {
        const suggestionsList = citySearchInput.parentNode.querySelector('ul');
        if (suggestionsList) {
            suggestionsList.remove();
        }
        return;
    }
}