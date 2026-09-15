export function getTheme() {
    return localStorage.getItem('theme') || 'system';
}

export function setTheme(theme) {
    localStorage.setItem('theme', theme);
    applyTheme(theme);
}

function applyTheme(theme) {
    const systemDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const isDark = theme === 'dark' || (theme === 'system' && systemDark);
    document.documentElement.classList.toggle('dark', isDark);
}

export function watchSystemTheme(dotNetRef) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
        if (getTheme() === 'system') {
            applyTheme('system');
            dotNetRef.invokeMethodAsync('OnSystemThemeChanged', e.matches);
        }
    });
}