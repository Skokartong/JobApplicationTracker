// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// filepath: c:\Users\SaltDev\salt\JobApplicationTracker\JobApplicationTracker\wwwroot\js\site.js

function toggleDesc(jobId) {
    var shortDesc = document.getElementById('desc-' + jobId);
    var fullDesc = document.getElementById('full-desc-' + jobId);
    var btn = document.getElementById('toggle-btn-' + jobId);  // Get the button element
    
    if (shortDesc.style.display === 'none') {
        shortDesc.style.display = 'block';
        fullDesc.style.display = 'none';
        btn.classList.remove('rotated');
    } else {
        shortDesc.style.display = 'none';
        fullDesc.style.display = 'block';
        btn.classList.add('rotated');
    }
}