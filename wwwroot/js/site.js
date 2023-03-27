// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function updateClock() {
    var now = new Date(), // current date
        months = ['Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli',
            'Augusti', 'September', 'Oktober', 'November', 'December',]; // you get the idea
    time = now.getHours() + ':' + now.getMinutes(), // again, you get the idea

        // a cleaner way than string concatenation
        date = [now.getDate(),
        months[now.getMonth()],
        now.getFullYear()].join(' ');

    // set the content of the element with the ID time to the formatted string
    document.getElementById('time').innerHTML = [date, time].join(' / ');

    // call this function again in 1000ms
    setTimeout(updateClock, 1000);
}
updateClock(); // initial call