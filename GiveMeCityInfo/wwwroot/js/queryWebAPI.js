const baseURI = 'https://localhost:7192/api/v2/cities/';

// Get all cities
function getCities(cb) {
    fetch(baseURI)
        .then(response => response.json())
        .then(data => { return cb(data) })
        .catch(error => console.error('Unable to get cities.', error));
}
function getCitiesByCountry(country, cb) {
    fetch(`${baseURI}?country=${country}`)
        .then(response => response.json())
        .then(data => { return data })
        .catch(error => console.error('Unable to get cities.', error));
}


// Get one city by ID
function getCityById(cityId) {
    fetch(`${baseURI}/${cityId}`)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error('Unable to get city.', error));
}

// Get cities by name (fuzzy)
function getCitiesByName(cityName) {
    fetch(`${baseURI}?name=${cityName}`)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error(`Unable to get city called ${cityName}.`, error));
}

// Get city by searchQuery (fuzzy)
function getCitiesBySearchQuery(searchQuery) {
    fetch(`${baseURI}?searchQuery=${searchQuery}`)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error(`No results found for query ${searchQuery}.`, error));
}

// Get a single point of interest provided a cityId
function getPointOfInterest(cityId) {
    fetch(`${baseURI}/${cityId}/pointsofinterest`)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error(`No results found for city ${cityId}.`, error));
}

// Get all points of interest provided a pointOfInterestId
function getPointsOfInterest(pointOfInterestId) {
    fetch(`${baseURI}/${pointOfInterestId}/pointsofinterest`)
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error(`No results found for city ${cityId} with point of interest id ${pointOfInterestId}.`, error));
}

function getCitiesByContinent(continentName, cb) {
    return fetch(`${baseURI}?continent=${continentName}`)
        .then(response => response.json())
        .then(data => { return cb(data) })
        .catch(error => console.error('Unable to get cities.', error));
}