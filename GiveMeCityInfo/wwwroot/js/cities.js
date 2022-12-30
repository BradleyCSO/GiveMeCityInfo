// Base URL to query the API
const baseURI = 'https://localhost:7192/api/v2/cities/';

// Update HTML elements
document.querySelector('button[type="submit"]').remove();

let selectedCountryOptions = []

fetch(`${baseURI}GetContinents`).then(response => response.json()).then(data => {
    addContinentsCheckboxes(Array.from(data))
}
).catch(error => console.error('Unable to get continents.', error));

// Event listeners
document.addEventListener('click', function (e) {
    // Update country list options based on selected continent value(s)
    if (e.target.id == 'continent') {
        // When continent changes, get values of all checked continent items and push them to array
        let continents = []
        document.querySelectorAll('input[type=checkbox]:checked').forEach(element => continents.push(element.value))

        // Get option HTML elements
        const countriesSelected = document.querySelectorAll('.countries-select');

        countriesSelected.forEach(checkbox => {
            toggleChildNodes(checkbox, continents);
        }
        );
    }
    else if (e.target.id == 'country') {
        let selectedCountries;

        if (e.target.options) {
            selectedCountries = [...e.target.options].filter(option => option.selected).map(option => option.value);
        } else {
            selectedCountries = [e.target.value];
        }

        selectedCountryOptions = selectedCountries;

        let queryString = encodeQueryString(selectedCountries);
        fetch(`${baseURI}?${queryString}`).then(response => {
            // Get pagination header and convert it into a JSON object
            const xPaginationHeader = JSON.parse(response.headers.get('X-Pagination'));

            buildPaginationButtons(xPaginationHeader.TotalPageCount)

            return response.json();
        }
        ).then(data => {
            // Update with new items
            updateCityCards(data);
        }
        ).catch(error => console.error('Unable to get countries.', error));
    }// Fetch paginated data providing value of selected page button element as parameter to API, update rendered elements accordingly
    else if (e.target.classList.contains('page-link')) {
        e.preventDefault();

        // Clear old active style
        document.querySelectorAll('.page-link').forEach(button => {
            button.classList.remove('active');
        });

        // Add active to current targeted pagination element
        e.target.classList.add('active');

        let queryString = encodeQueryString(selectedCountryOptions);

        // Fetch new items provided page number
        fetch(`${baseURI}?${queryString}&pageNumber=${e.target.innerText}`)
            .then(response => response.json())
            .then(data => {
                updateCityCards(data);
            })
            .catch(error => console.error('Unable to get cities.', error));
    }
});

function addContinentsCheckboxes(continents) {
    for (const continent in continents) {
        document.getElementsByClassName('continents')[0].innerHTML += `<label class="mb-2 form-check-label">${continents[continent]}</label>`
        document.getElementsByClassName('continents')[0].innerHTML += `<input type="checkbox" checked="checked" class="form-check-input me-4" name="continent" id="continent" value="${continents[continent]}"</input>`
    }
}

function encodeQueryString(queryString) {
    return queryString.map(country => `countries=${encodeURIComponent(country)}`).join('&');
}

function buildPaginationButtons(totalPageCount) {
    const paginationButtonsContainer = document.querySelector('.pagination-buttons-container');

    // Clear out old paginationButtons
    paginationButtonsContainer.innerHTML = '';

    // We only want the pagination buttons to show if there's more than one page
    if (totalPageCount > 1) {
        for (let i = 1; i <= totalPageCount; i++) {
            const ul = document.createElement('ul');
            ul.setAttribute('class', 'd-inline-flex m-1 pagination');

            const li = document.createElement('li');
            li.setAttribute('class', 'page-item');
                
            const a = document.createElement('a');
            a.innerHTML = `${i}`;
            a.setAttribute('class', 'page-link');

            ul.appendChild(li);
            li.appendChild(a);

            paginationButtonsContainer.appendChild(ul);
        }

        // Set first page as active button 
        document.querySelector('.page-link').setAttribute('class', 'page-link active');
    }
}

function toggleChildNodes(parent, filteredResults) {
    for (let child of parent.children) {
        // Check if the value of the child element's data-continent attribute is present in the filteredResults array
        if (filteredResults.includes(child.getAttribute('data-continent'))) {
            // If it is, set the display property to show
            child.style.display = 'block';
        } else {
            // Otherwise set it not to display
            child.style.display = 'none';
        }
    }
}

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}

function updateCityCards(cities) {
    // Clear old items
    removeAllChildNodes(document.querySelector('.city-items'));

    for (let i = 0; i < cities.length; i++) {
        let div = document.createElement('div');
        div.classList.add('text-center', 'card');
        div.setAttribute('data-country', cities[i].country);

        let img = document.createElement('img');
        img.classList.add('card__img');
        img.setAttribute('src', cities[i].imageUrl);
        img.setAttribute('alt', cities[i].imageAltText);

        let cardImgOverlay = document.createElement('div');
        cardImgOverlay.classList.add('card-img-overlay');

        let cardTitle = document.createElement('h5');
        cardTitle.classList.add('card-title', 'card__text', 'text-white');
        cardTitle.textContent = cities[i].name;

        let a = document.createElement('a');
        a.classList.add('stretched-link');
        a.setAttribute('href', '/city?cityId=' + cities[i].id);

        // Append <h5> element which contains city name to the cardImgOverlay div
        cardImgOverlay.appendChild(cardTitle);

        // Append <a> element which contains href attribute linking a City inside cardImgOverlay div
        cardImgOverlay.appendChild(a);

        // Append img which contains url and alt text for a given city
        div.appendChild(img);

        // Append the img overlay to the div
        div.appendChild(cardImgOverlay);

        // Append the created card div to the city-items container
        document.querySelector('.city-items').appendChild(div);
    }
}