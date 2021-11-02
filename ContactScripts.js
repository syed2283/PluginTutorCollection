var Sdk = window.Sdk || {};
(
    function () {
        this.formOnLoad = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var firstName = formContext.getAttribute("firstname").getValue();
            alert("Alhamdulillah! Fetched first name using form context : " + firstName);
        }
        this.firstNameOnChange = function (executionContext) {

        }
    }    
).call(Sdk);