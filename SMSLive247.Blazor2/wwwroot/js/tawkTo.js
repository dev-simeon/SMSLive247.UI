(function () {
    window.LoadTawk = function (propertyId) {
        if (!propertyId || document.getElementById("tawk-script")) {
            return;
        }

        window.Tawk_API = window.Tawk_API || {};
        window.Tawk_API.customStyle = {
            zIndex: 1000
        };
        window.Tawk_LoadStart = new Date();

        var script = document.createElement("script");
        script.id = "tawk-script";
        script.async = true;
        script.charset = "UTF-8";
        script.setAttribute("crossorigin", "*");

        var tawkUrl = propertyId.indexOf("https://embed.tawk.to/") === 0
            ? propertyId
            : "https://embed.tawk.to/" + propertyId + (propertyId.indexOf("/") >= 0 ? "" : "/default");

        script.src = tawkUrl + (tawkUrl.indexOf("?") >= 0 ? "&" : "?") + "layout=modern";

        var firstScript = document.getElementsByTagName("script")[0];
        if (firstScript && firstScript.parentNode) {
            firstScript.parentNode.insertBefore(script, firstScript);
        } else {
            document.head.appendChild(script);
        }
    };

    window.smsliveOpenTawkChat = function () {
        if (window.Tawk_API && typeof window.Tawk_API.showWidget === "function") {
            window.Tawk_API.showWidget();
            window.Tawk_API.maximize();
            return true;
        }

        alert("Live chat is still initializing. Please wait a moment and try again.");
        return false;
    };

    window.LoadTawk(window.tawkToPropertyId);
})();


