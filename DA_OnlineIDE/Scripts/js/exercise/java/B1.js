﻿// Retrieve Elements
const consoleLogList = document.querySelector('.editor__console-logs');
const executeCodeBtn = document.querySelector('.editor__run');
const resetCodeBtn = document.querySelector('.editor__reset');
const Lang = $(document).find(':selected').attr('data-id');

// Setup Ace
let codeEditor = ace.edit("editorCode");
let defaultCode = "class Main{\n\tpublic static void main(String[] args){\n\t\tSystem.out.print(" + '"Hello World"' + ");\n\t}\n}";
let consoleMessages = [];

let editorLib = {
    clearConsoleScreen() {
        consoleMessages.length = 0;

        // Remove all elements in the log list
        while (consoleLogList.firstChild) {
            consoleLogList.removeChild(consoleLogList.firstChild);
        }
    },
    printToConsole() {
        consoleMessages.forEach(log => {
            const newLogItem = document.createElement('li');
            const newLogText = document.createElement('pre');

            newLogText.className = log.class;
            newLogText.textContent = `> ${log.message}`;

            newLogItem.appendChild(newLogText);

            consoleLogList.appendChild(newLogItem);
        })
    },
    init() {
        // Configure Ace

        // Theme
        codeEditor.setTheme("ace/theme/dreamweaver");

        // Set language
        //codeEditor.session.setMode("ace/mode/java");
        var javaMode = require("ace/mode/java").Mode;
        codeEditor.getSession().setMode(new javaMode());



        // Set Options
        codeEditor.setOptions({
            fontFamily: 'Inconsolata',
            fontSize: '12pt',
            enableBasicAutocompletion: true,
            enableLiveAutocompletion: true,
        });

        // Set Default Code
        codeEditor.setValue(defaultCode);
    }
}



// Events
executeCodeBtn.addEventListener('click', () => {
    // Clear console messages
    editorLib.clearConsoleScreen();

    // Get input from the code editor
    const userCode = codeEditor.getValue();
    // Run the user code

    console.log(userCode);
    $.ajax({
        type: "post",
        url: "/Java/CompileCode",
        data: {
            code: userCode,
            lang: Lang
        },
        success: function (msg) {
            console.log(msg);
            document.getElementById("output").textContent = msg;
            $.ajax({
                type: "get",
                url: "/Java/check",
                success: function (msg) {
                    console.log(msg);
                    if (msg == "Correct") {
                        $("#correctModal").modal("toggle");
                    }
                    else {
                        $("#incorrectModal").modal("toggle");
                    }
                }
            });
        }
    });


    // Print to the console
    editorLib.printToConsole();
});

resetCodeBtn.addEventListener('click', () => {
    // Clear ace editor
    codeEditor.setValue(defaultCode);

    // Clear console messages
    editorLib.clearConsoleScreen();
})

editorLib.init();