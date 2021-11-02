var Sdk = window.Sdk || {};
(
    function () {
        this.formOnLoad = function (executionContext) {
            //calling global helper file functions
            Helper.DoSomething();

            var formContext = executionContext.getFormContext();

            //Form type example
            var formtype = formContext.ui.getFormType();

            if (formtype == 1)
                formContext.ui.setFormNotification("User is creating Account", "INFO", "1");
            else if (formtype==2)
                formContext.ui.setFormNotification("User is opening existing Account", "INFO", "2");
            else if (formtype == 3)
                formContext.ui.setFormNotification("User does not have permissions to edit the Account record", "INFO", "3");

            //handling lookup values
            //var lookupArray = formContext.getAttribute("parentcustomerid").getValue();

            //if (lookupArray[0]!=null) {
            //    var guidofAccount = lookupArray[0].id;
            //    var nameofAccount = lookupArray[0].name;
            //    var entitytype = lookupArray[0].entityType;

            //    formContext.ui.setFormNotification("GUID of the Account is " + guidofAccount, "INFO", "1");
            //    formContext.ui.setFormNotification("Name of the Account is " + nameofAccount, "INFO", "2");
            //    formContext.ui.setFormNotification("Entity Type of the Account is " + entitytype, "INFO", "3");
            //}

            //var owndership = formContext.getAttribute("ownershipcode").getText();
            //if (owndership) {
            //    formContext.getControl("ownershipcode").setNotification(owndership,"ownershipmsg");
            //} else {
            //    formContext.getControl("ownershipcode").clearNotification("ownershipmsg");
            //}
        }
        this.preventAutoSave = function (executionContext) {
            var eventArgs = executionContext.getEventArgs();
            if (eventArgs.getSaveMode() == 70) {
                eventArgs.preventDefault();
            }
        }
        this.MainPhoneOnChange = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var phoneNumber = formContext.getAttribute("telephone1").getValue();

            var expression = new RegExp("^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}");

            if (!expression.test(phoneNumber)) {
                //field notification
                formContext.getControl("telephone1").setNotification("Enter Phone number in UAE format", "telephonemsg");
                //form notification
                formContext.ui.setFormNotification("Enter Phone number in UAE format", "INFO", "formnoti1");
            } else {
                //clear notification
                formContext.getControl("telephone1").clearNotification("telephonemsg");
                formContext.ui.clearFormNotification("formnoti1");
            }
        }
    }
).call(Sdk);