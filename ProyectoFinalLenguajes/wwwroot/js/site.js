function applyTheme(theme) {
    
    document.documentElement.setAttribute('data-bs-theme', theme);

    
    const toggleButton = document.getElementById('darkModeToggle');
    if (toggleButton) {
        if (theme === 'dark') {
            toggleButton.innerHTML = '<i class="bi bi-sun-fill"></i>'; 
            toggleButton.setAttribute('title', 'Switch to light mode');
        } else {
            toggleButton.innerHTML = '<i class="bi bi-moon-stars-fill"></i>'; 
            toggleButton.setAttribute('title', 'Switch to dark mode');
        }
    }
}


document.addEventListener('DOMContentLoaded', () => {
    
    const savedTheme = localStorage.getItem('theme');

    
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    
    if (savedTheme) {
        applyTheme(savedTheme);
    } else if (prefersDark) {
        applyTheme('dark'); 
    } else {
        applyTheme('light'); 
    }

    
    const toggleButton = document.getElementById('darkModeToggle');
    if (toggleButton) {
        toggleButton.addEventListener('click', () => {
            let currentTheme = document.documentElement.getAttribute('data-bs-theme');
            let newTheme = (currentTheme === 'light') ? 'dark' : 'light';
            applyTheme(newTheme);
            localStorage.setItem('theme', newTheme); 
        });
    }

    
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
        
        if (!localStorage.getItem('theme')) {
            const newColorScheme = event.matches ? 'dark' : 'light';
            applyTheme(newColorScheme);
        }
    });
})