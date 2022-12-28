// Update HTML elements
document.querySelector('#submit').remove();

// Fetch data
let apiData;

let countryOptions = document.querySelectorAll("countries")

fetch(`${baseURI}`).then(response => response.json()).then(data => {
    apiData = data;
    addContinentsCheckboxes(Array.from(apiData))
}
).catch(error => console.error('Unable to get cities.', error));

function addContinentsCheckboxes(continents) {
    for (const continent in continents) {
        document.getElementsByClassName("continents")[0].innerHTML += `<label class="form-check-label">${continents[continent].continent}</label>`
        document.getElementsByClassName("continents")[0].innerHTML += `<input type="checkbox" checked="checked" class="form-check-input" name="continent" id="continent" value="${continents[continent].continent}"</input>`
    }
}

// Event listeners
document.addEventListener('click', function (e) {
    // Update country list and city item elements based on continent selected
    if (e.target.id == 'continent') {
        // When continent changes, get values of all checked continent items and push them to array
        let continents = []
        document.querySelectorAll('input[type=checkbox]:checked').forEach(element => continents.push(element.value))

        let results = []

        // Filter the results where city items match selected continent(s)
        if (continents.length > 0) {
            // Update option values based on selected continent(s)
            results = apiData.filter(element => {
                return continents.some(continent => element.continent.includes(continent))
            }
            )
        }

        // Clear old items
        toggleChildNodes(document.querySelector('.countries-select'), results);

        // Update the rendered HTML
        //updateHtml(results)
    } else if (e.target.id == 'country') {
    }
    // Fetch paginated data providing value of selected page button element as parameter to API, update rendered elements accordingly
    else if (e.target.classList.contains('paginationButtons')) {
        e.preventDefault();
        document.querySelectorAll('.paginationButtons').forEach(button => {
            button.addEventListener('click', event => {
                // Clear old styles
                document.querySelectorAll('.paginationButtons').forEach(b => b.classList.remove('active'));

                // Add active to current targeted pagination element
                event.currentTarget.classList.add('active');

                // Clear old items
                removeAllChildNodes(document.querySelector('.city-items'));

                // Fetch new items provided page number
                fetch(`${baseURI}?pageNumber=${event.currentTarget.innerText}`).then(response => response.json()).then(data => {
                    //updateHtml(data)
                }
                ).catch(error => console.error('Unable to get cities.', error));
            }
            );
        }
        );
    }
});

function toggleChildNodes(parent, filteredResults) {
    // for (let child of parent.children) {     
    //     console.log(filteredResults.includes(child.getAttribute("data-continent")))
    //     if (filteredResults.includes(child.getAttribute("data-continent"))) {
    //         child.style.display = "none;"
    //     }
    //     else {
    //         child.style.display = "block";
    //     }
    // }
}

function updateHtml(results, updateCountries) {
    for (let i = 0; i < results.length; i++) {
        document.getElementsByClassName('countries-select')[0].innerHTML += `<option value=${results[i].country} class='form-check-label'>${results[i].country}</option>`
        document.getElementsByClassName('city-items')[0].innerHTML += `<p>${results[i].name}</p>`
    }
}