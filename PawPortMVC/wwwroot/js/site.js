let tamanoFuenteActual = parseInt(localStorage.getItem("pawport_font_size") || "16", 10);

function aplicarPreferenciasAccesibilidad() {
    document.documentElement.style.setProperty("--base-font-size", tamanoFuenteActual + "px");

    const modoOscuro = localStorage.getItem("pawport_dark_mode") === "1";
    const altoContraste = localStorage.getItem("pawport_high_contrast") === "1";

    document.body.classList.toggle("dark-mode", modoOscuro);
    document.body.classList.toggle("high-contrast", altoContraste);

    const btnOscuro = document.getElementById("btnModoOscuro");
    const btnContraste = document.getElementById("btnContraste");
    if (btnOscuro) btnOscuro.classList.toggle("active", modoOscuro);
    if (btnContraste) btnContraste.classList.toggle("active", altoContraste);
}

function cambiarFuente(direccion) {
    tamanoFuenteActual = Math.min(22, Math.max(13, tamanoFuenteActual + direccion * 2));
    localStorage.setItem("pawport_font_size", tamanoFuenteActual.toString());
    aplicarPreferenciasAccesibilidad();
}

function alternarModoOscuro() {
    const activo = localStorage.getItem("pawport_dark_mode") === "1";
    localStorage.setItem("pawport_dark_mode", activo ? "0" : "1");
    if (!activo) localStorage.setItem("pawport_high_contrast", "0");
    aplicarPreferenciasAccesibilidad();
}

function alternarAltoContraste() {
    const activo = localStorage.getItem("pawport_high_contrast") === "1";
    localStorage.setItem("pawport_high_contrast", activo ? "0" : "1");
    if (!activo) localStorage.setItem("pawport_dark_mode", "0");
    aplicarPreferenciasAccesibilidad();
}

function restablecerAccesibilidad() {
    tamanoFuenteActual = 16;
    localStorage.removeItem("pawport_font_size");
    localStorage.removeItem("pawport_dark_mode");
    localStorage.removeItem("pawport_high_contrast");
    aplicarPreferenciasAccesibilidad();
}

document.addEventListener("DOMContentLoaded", aplicarPreferenciasAccesibilidad);