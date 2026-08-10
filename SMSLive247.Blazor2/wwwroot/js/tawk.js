(function () {
    window.smsliveLoadTawk = function (propertyId) {
        if (!propertyId || document.getElementById("tawk-script")) {
            return;
        }

        window.Tawk_API = window.Tawk_API || {};
        window.Tawk_LoadStart = new Date();

        window.Tawk_API.onLoad = function () {
            if (typeof window.Tawk_API.hideWidget === "function") {
                window.Tawk_API.hideWidget();
            }
        };

        window.Tawk_API.onChatMinimized = function () {
            if (typeof window.Tawk_API.hideWidget === "function") {
                window.Tawk_API.hideWidget();
            }
        };

        var script = document.createElement("script");
        script.id = "tawk-script";
        script.async = true;
        script.charset = "UTF-8";
        script.setAttribute("crossorigin", "*");

        var widgetId = propertyId.indexOf("/") >= 0 ? "" : "/1";
        script.src = "https://embed.tawk.to/" + propertyId + widgetId;

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
})();
