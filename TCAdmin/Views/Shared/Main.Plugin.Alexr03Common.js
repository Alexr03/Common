var Alexr03 = {};

Alexr03.Common = function Alexr03$Common() {

    function _handleAjaxSuccess(e, okFunction) {
        if(!isFunction(okFunction)){
            okFunction = function(e) {}
        }
        if (e.Message) {
            TCAdmin.Ajax.ShowBasicDialog("Success!", e.Message, okFunction);
        }

        kendo.ui.progress($(document.body), false)
    }

    function _handleAjaxError(e, okFunction) {
        if(!isFunction(okFunction)){
            okFunction = function(e) {}
        }
        if (e.responseJSON && e.responseJSON.Message) {
            TCAdmin.Ajax.ShowBasicDialog("Error!", e.responseJSON.Message, okFunction);
        } else {
            TCAdmin.Ajax.ShowBasicDialog("Error!", "An error has occured! Please try again later.", okFunction)
        }

        kendo.ui.progress($(document.body), false)
    }

    function _handleAjaxBegin() {
        kendo.ui.progress($(document.body), true)
    }

    function _handleAjaxComplete() {
        kendo.ui.progress($(document.body), false)
    }

    function isFunction(functionToCheck) {
        return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
    }

    return {
        HandleAjaxSuccess: _handleAjaxSuccess,
        HandleAjaxFailure: _handleAjaxError,
        HandleAjaxBegin: _handleAjaxBegin,
        HandleAjaxComplete: _handleAjaxComplete,
    }
}();