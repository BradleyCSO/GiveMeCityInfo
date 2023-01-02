// Get HTML elements
const leftButton = document.getElementById('button-left');
const rightButton = document.getElementById('button-right');
const scrollableContainer = document.querySelector('.scrollable-items');

// Hide the scrollbar
scrollableContainer.style.overflow = 'hidden';

// Set scroll params
const scrollSpeed = 750;
const scrollInterval = 50;
const scrollDistance = scrollSpeed * scrollInterval / 1000;

// Flag to track whether the button is being held down
let isScrolling = false;

leftButton.addEventListener('mousedown', () => {
    isScrolling = true;
    scrollLeft();
});
leftButton.addEventListener('mouseup', () => {
    isScrolling = false;
});

rightButton.addEventListener('mousedown', () => {
    isScrolling = true;
    leftButton.removeAttribute('disabled');
    scrollRight();
});
rightButton.addEventListener('mouseup', () => {
    isScrolling = false;
});

function scrollLeft() {
    if (isScrolling) {
        scrollableContainer.scrollLeft -= scrollDistance;
        setTimeout(scrollLeft, scrollInterval);
        rightButton.removeAttribute('disabled');
    }
    if (scrollableContainer.scrollLeft == 0) {
        leftButton.setAttribute('disabled', '');
    }
}

function scrollRight() {
    if (isScrolling) {
        scrollableContainer.scrollLeft += scrollDistance;
        setTimeout(scrollRight, scrollInterval);
    }
    if (scrollableContainer.scrollWidth - scrollableContainer.scrollLeft - scrollableContainer.clientWidth <= 1) {
        rightButton.setAttribute('disabled', '')
    }
}