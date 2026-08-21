window.ContentEditor = {
    insertChip: function (elementId, chipHtml) {
        var el = document.getElementById(elementId);
        if (!el) return;

        el.focus();
        
        var sel = window.getSelection();
        var range;
        
        if (sel.getRangeAt && sel.rangeCount) {
            range = sel.getRangeAt(0);
            
            // Make sure the selection is inside the editor
            if (!el.contains(range.commonAncestorContainer)) {
                range = document.createRange();
                range.selectNodeContents(el);
                range.collapse(false); // collapse to end
                sel.removeAllRanges();
                sel.addRange(range);
            }
        } else {
            // Fallback
            range = document.createRange();
            range.selectNodeContents(el);
            range.collapse(false);
            sel.removeAllRanges();
            sel.addRange(range);
        }

        var tempDiv = document.createElement("div");
        tempDiv.innerHTML = chipHtml;
        var frag = document.createDocumentFragment();
        var node, lastNode;
        
        while ((node = tempDiv.firstChild)) {
            lastNode = frag.appendChild(node);
        }
        
        // Add a zero-width space after the chip so the cursor can move past it
        var zwsp = document.createTextNode('\u200B');
        frag.appendChild(zwsp);

        range.insertNode(frag);

        // Preserve the selection
        if (zwsp) {
            range = range.cloneRange();
            range.setStartAfter(zwsp);
            range.collapse(true);
            sel.removeAllRanges();
            sel.addRange(range);
        }
        
        // Trigger an input event to notify Blazor of changes (if we bind to oninput)
        var event = new Event('input', { bubbles: true });
        el.dispatchEvent(event);
    },
    
    getRawText: function (elementId) {
        var el = document.getElementById(elementId);
        if (!el) return "";
        
        var text = "";
        for (var i = 0; i < el.childNodes.length; i++) {
            var node = el.childNodes[i];
            
            if (node.nodeType === Node.TEXT_NODE) {
                text += node.textContent;
            } else if (node.nodeType === Node.ELEMENT_NODE) {
                if (node.nodeName.toLowerCase() === "br") {
                    text += "\n";
                } else if (node.hasAttribute("data-value")) {
                    text += node.getAttribute("data-value");
                } else if (node.nodeName.toLowerCase() === "div" || node.nodeName.toLowerCase() === "p") {
                    // Block elements implicitly add newlines
                    if (i > 0) text += "\n";
                    text += node.innerText || node.textContent;
                } else {
                    text += node.innerText || node.textContent;
                }
            }
        }
        
        // Remove zero-width spaces
        return text.replace(/\u200B/g, '');
    },
    
    initialize: function (elementId, dotNetHelper) {
        var el = document.getElementById(elementId);
        if (!el) return;
        
        el.addEventListener('input', function() {
            var rawText = window.ContentEditor.getRawText(elementId);
            dotNetHelper.invokeMethodAsync('OnContentChanged', rawText);
        });
        
        // Prevent enter from creating nested divs if possible, just insert BR
        el.addEventListener('keydown', function(e) {
            if (e.key === 'Enter') {
                document.execCommand('insertLineBreak');
                e.preventDefault();
            }
        });
    }
};
