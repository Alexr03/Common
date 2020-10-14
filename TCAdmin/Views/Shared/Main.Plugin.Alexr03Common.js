var Alexr03 = {};

Alexr03.Common = function Alexr03$Common() {

    function _handleAjaxSuccess(e, okFunction) {
        if(!isFunction(okFunction)){
            toastr["success"]("Success", e.Message)
        }else{
            if (e.Message) {
                TCAdmin.Ajax.ShowBasicDialog("Success!", e.Message, okFunction);
            }
        }

        _handleAjaxComplete();
    }

    function _handleAjaxError(e, okFunction) {
        if(!isFunction(okFunction)){
            toastr["success"]("Success", e.responseJSON.Message)
        }
        else{
            if (e.responseJSON && e.responseJSON.Message) {
                TCAdmin.Ajax.ShowBasicDialog("Error!", e.responseJSON.Message, okFunction);
            } else {
                TCAdmin.Ajax.ShowBasicDialog("Error!", "An error has occured! Please try again later.", okFunction)
            }
        }

        _handleAjaxComplete();
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