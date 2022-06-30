if (!String.prototype.startsWith) {
    Object.defineProperty(String.prototype, 'startsWith', {
        value: function (search, rawPos) {
            var pos = rawPos > 0 ? rawPos | 0 : 0;
            return this.substring(pos, pos + search.length) === search;
        }
    });
}





if (!navegadorValido()) window.location.href = "/Error/NavegadorNoSoportado";

function navegadorValido() {
    if (typeof detect === "undefined")
        return true;

    var navegador = obtenerNavegador();

    if (navegador.type === 'Mobile') return true;
    if (navegador.family === 'Chrome') return true;
    if (navegador.family === 'Firefox') return true;
    if (navegador.family === 'IE' && navegador.version >= 10) return true;
    return false;
}

function obtenerNavegador() {
    var user = detect.parse(navigator.userAgent);
    var isEdge = user.source.indexOf('Edge', 0) != -1;
    var otherNavigator = user.browser.family;

    if (otherNavigator === 'Chrome' && isEdge)
        return {
            "family": "Edge",
            "version": user.browser.version,
            "type": user.device.type
        };

    return {
        "family": user.browser.family,
        "version": user.browser.version,
        "type": user.device.type
    };
}


function mostrarAjaxError(xhr, status, error) {
    var mensaje = '';

    if (xhr.status == 0 || xhr.status == 408) {
        mensaje += 'No se pudo establecer conexión con el servidor.<br>Favor verifique su conexión a internet.';
        if (xhr.status == 408) {
            mensaje += '<br>Código: ' + xhr.status;
        }
    } else if (xhr.status >= 400 && xhr.status < 500) {
        mensaje += 'Operación Incorrecta o No autorizada.';
        mensaje += '<br>Código: ' + xhr.status;
    } else if (xhr.status >= 500) {
        mensaje += 'Ocurrió un <b>error interno</b> en el servidor.';
        mensaje += '<br>Código: ' + xhr.status;
    } else {
        mensaje += 'No se pudo completar la operación.';
        mensaje += '<br>Código: ' + xhr.status;
    }

    showAFPModal({
        type: 'd',
        title: 'Error',
        message: mensaje
    });
}

function setTitle(title) {
    localStorage.setItem("Title", title);
}

function validarRuc(ruc) {

    if (isNaN(ruc))
        return false;

    if (ruc.length != 11) {
        return false;
    }

    var di = ruc.split("");
    var sumRuc = eval(di[0]) * 5 + eval(di[1]) * 4 + eval(di[2]) * 3 + eval(di[3]) * 2 + eval(di[4]) * 7 + eval(di[5]) * 6 + eval(di[6]) * 5 + eval(di[7]) * 4 + eval(di[8]) * 3 + eval(di[9]) * 2;
    var ultimoDigitoRuc = eval(di[10]);

    var enteroRuc = parseInt(sumRuc / 11);
    var enteroRuc11 = enteroRuc * 11;
    var difSumRucEnteroRuc11 = sumRuc - enteroRuc11;
    var dif11DifSumRucEnteroRuc11 = 11 - difSumRucEnteroRuc11;

    if (dif11DifSumRucEnteroRuc11 == 10) {
        digitoVerificador = 0;
    } else if (dif11DifSumRucEnteroRuc11 == 11) {
        digitoVerificador = 1;
    } else {
        digitoVerificador = dif11DifSumRucEnteroRuc11;
    }

    if (digitoVerificador == ultimoDigitoRuc) {
        return true;
    } else {
        return false;
    }
}


function establecerFreezeGrilla() {


    if (typeof detect === 'undefined') return; //Se verifica si el archivo detect se ha cargado.

    var navegador = obtenerNavegador(); //Se obtiene el navegador, para validar si se va a ejecutar  en IE
    if (navegador.family !== 'IE') return; //Si el browser no es un IE se cancela el proceso de clonación de cabecera.

    if ($('.table-header-fixed > .table').length <= 0) return;  //si no se visualiza la grilla, se cancela el proceso de clonación.
    //if ($('.nofreeze').length > 0) return; // Si se encuentra la clase de nofreeze se cancela el proceso de clonación de cabecera.
    
    $('.custom_pager').addClass('overflow-300'); // se añade la clase overflow para especificar el limite de la cabecera clonada.

    if ($('thead.header').length <= 0)
        $('.table-header-fixed > .table > thead').addClass('header'); //se añade la clase header, para especificar la cabecera que se va a clonar..

    $('.table-header-fixed > .table').fixedHeader(); //Se ejecuta la funcion de clonación de cabecera.
}


(function ($) {

    (function ($) {
        $.fn.hasScrollBar = function () {
            return this.get(0).scrollHeight > this.height();
            //return this.get(0).scrollHeight > this.get(0).clientHeight;
        }
    })(jQuery);

    //proceso de clonación de cabecera que emula el freeze de la cabecera de la grilla.
    $.fn.fixedHeader = function (options)
    {
        var config = { topOffset: 36, leftOffset: 0, newtopOffset: 0 };//36
        if (options) { $.extend(config, options); }

        return this.each(function () {
            var o = $(this);

            if ($(this).hasClass('nofreeze')) return;
            //se evalua que no se vuelva a repetir el proceso de clonacion de cabecera.
            if ($(this).hasClass('head-clone')) return;
            $(this).addClass('head-clone');
           

            var $parent = $('.table-header-fixed'), 
                $window = $(window),
                $head = $('thead.header', o),
                $table = $('table.table', o),
                $botonCollapse = $('.desplazar', o),
                isFixed = 0;

            var headTop = $head.length && $head.offset().top; //- $head.height(); // -  config.topOffset;
            
            if ($head.data('topoffset') !== undefined) config['topOffset'] = $head.data('topoffset');
            else {
                if ($('.custom_pager').offset() === undefined)
                    config['topOffset'] = $('.table').offset().top - 0;
                else
                    config['topOffset'] = $('.table').offset().top - $('.custom_pager').offset().top;//$head.height();
            }

            if ($head !== undefined && $head.offset() !== undefined) {
                config['leftOffset'] = $head.offset().left;

            }

            function processScroll()
            {
                if (!o.is(':visible')) return;
                config['leftOffset'] = $head.offset().left;
                $('thead.header-copy', o).offset({ left: config['leftOffset'] });
            }

            function processResize() {

                config['topOffset'] = $('.table').offset().top - $('.custom_pager').offset().top;
                if (config['topOffset'] > 0 && $parent.scrollTop() == 0) {
                    $('thead.header-copy', o).css({ 'top': config['topOffset'] });
                }
            }

            // establece el ancho de las columnas, cuando esta se ha descuadrado.
            function headerCopyRectify() {
                o.find('thead.header > tr > th').each(function (i, h) {
                    var w = $(h).width();
                    o.find('thead.header-copy> tr > th:eq(' + i + ')').width(w);

                    var ancho = o.find('tbody > tr:first-child td:nth-child(' + (i + 1) + ')').outerWidth();
                });
            }

            function DesplazarFilas() {
                $('.table').find('thead.header > tr > th').each(function (i, h) {
                    var w = $(h).width();
                    $('.table').find('thead.header-copy> tr > th:eq(' + i + ')').width(w)
                });
            }

            $parent.on('scroll', processScroll);
            $window.on('resize', processResize);

            $table.resize(function () {
                console.log('processResize');
            });

            $head.clone(true).removeClass('header')
                .addClass('header-copy header-fixed')
                .css({ 'position': 'absolute' })
                .offset({ 'top': config['topOffset'], 'left': config['leftOffset']})
                .appendTo(o);



            o.find('thead.header-copy').width($head.width());

            headerCopyRectify();
            $head.css({
                margin: '0 auto',
                width: o.width(),
                'background-color': config.bgColor
            });
            processScroll();

        });

    };

})(jQuery);




var arrayDevengues = [];
$(function() {

    if ($('[data-control=periodo-devengue][data-rango=inicio]').length > 0 || $('[data-control=periodo-devengue][data-rango=fin]').length > 0) {
        $('[data-control="periodo-devengue"][data-rango="fin"] option').each(function (idx, item) {
            arrayDevengues.push({ value: item.value, text: item.text });
        });

        llenarDevengueFin($('[data-control=periodo-devengue][data-rango=inicio]'));
    }

    $('input[type="file"]').on('change', function() {
        var fileName = $(this).val();
        $(this).next('.custom-file-label').html(fileName.replace("C:\\fakepath\\", ""));
    });


    // Inicialización de las validaciones
    if ($('form').length > 0) {
        var validator = $.data($('form')[0], 'validator');

        if (validator) {
            var settngs = validator.settings;
            settngs.ignore = ".ignore";
        }

        $.validator.unobtrusive.parse($('form'));
    }

    // Inicialización de los DatePickers
    inicializarDatePicker();
});

function mostrarLoader() {
    var lastZIndex = getLastZIndex();
    $('#loader').css('z-index', lastZIndex + 1).fadeIn();
    $('body').addClass('loading');
}

function esconderLoader() {
    $('#loader').fadeOut();
    $('body').removeClass('loading');
    //funcion que establecera el freeze en la grilla de resultado.
    establecerFreezeGrilla();
    ResetearSesion();
}

function ResetearSesion() {
    if ($("#btnresetsession").length > 0)
        $("#btnresetsession").trigger("click");
}

$(document).on('click', '.btn-lnk-modal', function() {
    var url = $(this).attr('data-url');
    mostrarModal(url);
});

$(document).on('keydown', '.integer', function(e) {

    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: Ctrl+V
        (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});

$(document).on('click', '.page-link', function() {
    var targetForm = $(this).parents('.pager-container').data('form-container');
    var targetPage = $(this).data('page-number');
    $(targetForm).find('#pageNumber').val(targetPage);
    $(targetForm).submit();
});

// Datepicker
function inicializarDatePicker(control) {
    var elemento = control == null ? '.datepicker.nuevo' : control;
    $(elemento).datepicker({
        autoclose: true,
        format: 'dd/mm/yyyy',
        todayHighlight: true,
        language: 'es'
    });

    $(elemento).removeClass('nuevo');
}

$(document).on('click', '.datepicker', function() {
    $(this).val('').trigger('change');
});

$(document).on('change', '.datepicker[data-rango="inicio"]', function() {
    var $fechaFin = $(this)
        .parents('.form-group')
        .find('.datepicker[data-rango="fin"]');

    if ($fechaFin) {
        var fechaActual = $(this).data('datepicker').getDate();
        $fechaFin.data('datepicker').setStartDate(fechaActual);
    }
})

$(document).on('change', '.datepicker[data-rango="fin"]', function() {
    var $fechaInicio = $(this)
        .parents('.form-group')
        .find('.datepicker[data-rango="inicio"]');

    if ($fechaInicio) {
        var fechaActual = $(this).data('datepicker').getDate();
        $fechaInicio.data('datepicker').setEndDate(fechaActual);
    }
})






// Formularios
$(document).on('change', 'form [data-val-required][data-rango]', function () {

    var $elementos = $(this)
        .parents('.form-group')
        .find('[data-val-required][data-rango]');
    var conValor = false;

    $.each($elementos, function() {
        if ($(this).val() != '') {
            conValor = true;
            return;
        }
    });

    if (conValor) {
        $elementos.removeClass('ignore');
    } else {
        $elementos.addClass('ignore');
    }

    reiniciarValidacionesFormulario();
    $(this).parents('form').valid();
});




function configurarValidarDocumentoIdentidad(input, tipoDoc2) {
    if (typeof input === "string") {
        if (input.indexOf("#") > -1) { input = $(input); }
        else input = $("#" + input); 
    }

    var inputselect = input;
    var digitos = $(inputselect).data("digitos") === undefined ? 10 : parseInt($(inputselect).data("digitos"));
    var tipoDoc = tipoDoc2 === undefined ? $(input).val() : tipoDoc2;
    var validarTipo = $(input).data("validar-tipo");
    var oldvalue = $(inputselect).data("oldvalue") !== undefined ? $(inputselect).data("oldvalue") : 0;
    var input = $(input).data("validar-tipo-campo");
   
   

    $(input).attr("maxlength", "0");
    $(input).data("val-length-max", "0");

    if (validarTipo != "DocumentoIdentidad") return;
    $(input).removeClass('val-regex-solo-numeros val-regex-alfanumero').removeAttr('minlength').removeAttr('maxlength');//.removeAttr('solonumeroPaste');

    if (tipoDoc != null && tipoDoc != undefined) {
        var tipos = ['0', '1', '2', '3', '4', '5', '6', '9'];
        var tipo = tipos.indexOf(tipoDoc);

        if (tipo > -1)
        {
            if (oldvalue !== parseInt(tipoDoc)) {
                $(input).val("");
            }

            if (tipoDoc == '0') //DNI
                $(input).addClass('val-regex-solo-numeros').attr("maxlength", 8).attr('data-val-length-max', 8);
            else if (tipoDoc == "1") //CE
                $(input).addClass('val-regex-solo-numeros').attr("maxlength", 9).attr('data-val-length-max', 9);
            else if (tipoDoc == '2') //CMP
                $(input).addClass('val-regex-solo-numeros').attr('maxlength', 10).attr('data-val-length-max', 10);
            else if (tipoDoc == '3') //LAT
                $(input).addClass('val-regex-solo-numeros').attr('maxlength', digitos).attr('data-val-length-max', digitos);
            else if (tipoDoc == '4') //PAS
                $(input).addClass('val-regex-alfanumero').attr('maxlength', digitos).attr('data-val-length-max', digitos);
            else if (tipoDoc == '5') //INX
                $(input).addClass('val-regex-solo-numeros').attr('maxlength', 10).attr('data-val-length-max', 10);
            else if (tipoDoc == '6') //PTP
                $(input).addClass('val-regex-solo-numeros').attr('maxlength', digitos).attr('data-val-length-max', digitos);
            else if (tipoDoc == '9') //OTR
                $(input).addClass('val-regex-solo-numeros').attr('maxlength', 10).attr('data-val-length-max', 10);
        }
    }
}

//====== ListGroups - Inicio ======//

function AgregarObjetoHaciaArray(array, id, text) {
    var item = {
        Codigo: id,
        Descripcion: text
    };
    array.push(item);
};

function EliminarObjetoDesdeArray(array, id) {
    for (var j = array.length; j > 0; j--) {
        if (array[j - 1].Codigo == id) {
            array.splice(j - 1, 1);
        }
    }
};

function OrdenarArrayDeObjetos(array) {
    if (array.length > 0) {
        array.sort(function(a, b) {
            var nameA = a.Descripcion.toLowerCase(),
                nameB = b.Descripcion.toLowerCase()
            if (nameA < nameB) //sort string ascending
                return -1
            if (nameA > nameB)
                return 1
            return 0 //default return value (no sorting)
        });
    }
};

function GenerarListGroupItems(array) {
    OrdenarArrayDeObjetos(array);
    var lista = '';
    for (i = 0; i <= array.length - 1; i++) {
        lista += '<li class="list-group-item" data-id="' + array[i].Codigo + '">' + array[i].Descripcion + '</li>';
    }
    return lista;
}

function RefrescarListGroups() {
    $('.list-group.checked-list-box .list-group-item').each(function() {
        // Settings
        var $widget = $(this),
            $checkbox = $('<input type="checkbox" class="hidden" />'),
            color = ($widget.data('color') ? $widget.data('color') : "primary"),
            style = ($widget.data('style') == "button" ? "btn-" : "list-group-item-"),
            settings = {
                on: {
                    icon: 'glyphicon glyphicon-check'
                },
                off: {
                    icon: 'glyphicon glyphicon-unchecked'
                }
            };

        $widget.css('cursor', 'pointer')
        $widget.append($checkbox);

        // Event Handlers
        $widget.on('click', function() {
            $checkbox.prop('checked', !$checkbox.is(':checked'));
            $checkbox.triggerHandler('change');
            updateDisplay();
        });
        $checkbox.on('change', function() {
            updateDisplay();
        });


        // Actions
        function updateDisplay() {
            var isChecked = $checkbox.is(':checked');

            // Set the button's state
            $widget.data('state', (isChecked) ? "on" : "off");

            // Set the button's icon
            $widget.find('.state-icon')
                .removeClass()
                .addClass('state-icon ' + settings[$widget.data('state')].icon);

            // Update the button's color
            if (isChecked) {
                $widget.addClass(style + color + ' active');
            } else {
                $widget.removeClass(style + color + ' active');
            }
        }

        // Initialization
        function init() {

            if ($widget.data('checked') == true) {
                $checkbox.prop('checked', !$checkbox.is(':checked'));
            }

            updateDisplay();

            // Inject the icon if applicable
            if ($widget.find('.state-icon').length == 0) {
                $widget.prepend('<span class="state-icon ' + settings[$widget.data('state')].icon + '"></span>');
            }
        }
        init();
    });
}
//====== ListGroups - Fin ======//

/**
 * Collapse panels
 */
$(document).on('click', '.well.table-collapse-body', function() {
    $(this).find('i').toggleClass('glyphicon-plus');
    $(this).find('i').toggleClass('glyphicon-minus');
});

// Forms
function reiniciarValidacionesFormulario(formId) {

    var form = "form";
    if (formId) {
        form = formId;
    }

    var $form = $(form);
    var $validator = $form.validate();
    var $errors = $form.find(".field-validation-error span");

    $validator.resetForm();

    $form.find(".field-validation-error").removeClass("field-validation-error").addClass("field-validation-valid").find("span").remove();
}

function limpiarValoresFormulario(formId) {

    var form = "form";

    if (formId) {
        form = formId;
    }

    var $form = $(form);
    $(form).find('input[type="text"], input[type="password"], input[type="file"]').val('');
    $(".custom-file-label").html('');
    $(form).find('input[type="checkbox"].chk-form-section, input[type="radio"].chk-form-section').prop("checked", false);
    $(form).find('select').prop('selectedIndex', 0);
}



function alphanumeric(val) {
    var regex = /^[ 0-9a-zA-Z]+$/;
    if (regex.test(val)) {
        return true;
    } else {
        return false;
    }
}

function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}

function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

}

$(document).on("keypress", '.ruc', function(evt) {

    var ruc = $(this).val();

    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (!((charCode == 8 || charCode == 46 || charCode == 37 || charCode == 39 || charCode == 17 || charCode == 86) || (charCode >= 48 && charCode <= 57))) return false;


    if (ruc.length >= 11) {
        if (charCode == 8 || charCode == 46 || charCode == 37 || charCode == 39 || charCode == 17 || charCode == 86) return true;
        else return false;
    }

});


/* ----- Secuencia de combos de UBIGEO ----- */
/* Ver ejemplo en vista SolicitudAfiliacionTrabajador.cshtml (ver clases, data-) */

$(document).on('change', '.combo-departamento', function() {

    var $comboProvincia = $($(this).data('target'));
    var $comboDistrito = $($comboProvincia.data('target'));

    var url = $comboProvincia.data('url');
    var valor = $(this).val();

    $comboProvincia.html(optionTemplateEmpty);
    $comboDistrito.html(optionTemplateEmpty);

    $.ajax({
        url: url,
        data: {
            parametro: valor
        }
    }).done(function(data) {

        $.each(data, function() {
            var option = optionTemplate
                .replace('{0}', this.value)
                .replace('{1}', this.text);
            $comboProvincia.append(option);
        });

        if (valor != '') {
            $comboProvincia.removeAttr('disabled');
            $comboDistrito.attr('disabled', true);
        } else {
            $comboProvincia.attr('disabled', true);
            $comboDistrito.attr('disabled', true);
        }
    });

});

$(document).on('change', '.combo-provincia', function() {

    var $comboDistrito = $($(this).data('target'));
    var $comboDepartamento = $($comboDistrito.data('parent'));

    var url = $comboDistrito.data('url');
    var codDepartamento = $comboDepartamento.val();
    var codProvincia = $(this).val();
    var valor = codDepartamento + codProvincia;

    $comboDistrito.html(optionTemplateEmpty);

    $.ajax({
        url: url,
        data: {
            parametro: valor
        }
    }).done(function(data) {

        $.each(data, function() {
            var option = optionTemplate
                .replace('{0}', this.value)
                .replace('{1}', this.text);
            $comboDistrito.append(option);
        });

        if (codProvincia != '') {
            $comboDistrito.removeAttr('disabled');
        } else {
            $comboDistrito.attr('disabled', true);
        }
    });

});

/* --------- Manejo de campos opcionales dependiendo de otros --------- */
/* Ver ejemplo en vista SolicitudAfiliacionTrabajador.cshtml (ver clases, data-) */

$(document).on('change', '.campo-opcional', function() {
    var $campoTarget = $($(this).data('ignore-target'));
    var value = $(this).val();
    if (value == '') {
        $campoTarget.removeClass('ignore');
    } else {
        $campoTarget.addClass('ignore');
    }
    reiniciarValidacionesFormulario();
    $('form').valid();
});

/* --------- Control de ingreso de sólo números, backspace y delete --------- */
/* Ver ejemplo en vista SolicitudAfiliacionTrabajador.cshtml (ver clases, data-)*/
$(document).on('keypress', '.integer', function(e) {
    var val = /^[0-9\b\d]+$/.test(e.key);
    if (!val) {
        return false;
    }
});

$(document).on('keydown', '.alfanumeric', function(e) {
    var val = /^[0-9A-Za-zÑ]+$/.test(e.key);
    if (!val) {
        return false;
    }
});

$(document).on('keydown', '.alfanumericSpace', function(e) {
    var val = /^[A-Za-z\sÑ'Ü]+$/.test(e.key);
    if (!val) {
        return false;
    }
});

$(document).on("keypress", '.alfanumericoSigno', function(evt) {

    var regex = new RegExp("^[0-9a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ_ ]+$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});

$(document).on("keypress", '.alfanumericoSignoFull', function(evt) {

    var regex = new RegExp("^[0-9a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ_'-/. ]+$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});


/*
$(document).on("keyup", '.alfanumerico', function(evt) {

    var strCadena = new String($(this).val());
    if (strCadena == "") return true;

    var strCadenaFinal = strCadena;
    var valido = "ABCDEFGHIJKLMNÑOPQRSTUVWYXZabcdefghijklmnñopqrstuvwxyzáéíóú1234567890ÁÉÍÓÚ ";
    for (var i = 0; i <= strCadena.length - 1; i++) {
        if (valido.indexOf(strCadena.substring(i, i + 1), 0) == -1) {
            strCadenaFinal = strCadenaFinal.replace(strCadena.substring(i, i + 1), "");
        }
    }

    $(this).val(strCadenaFinal);
    return true;
});

$(document).on("keypress", '.alfanumerico', function(evt) {

    var regex = new RegExp("^([a-zA-Z0-9 ]+)$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});


$(document).on("keypress", '.solonumero', function(evt) {
    var regex = new RegExp("^([0-9]+)$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});

$(document).on("keyup", '.solonumero', function (evt) {

    var strCadena = new String($(this).val());
    if (strCadena == "") return true;

    var strCadenaFinal = strCadena;
    var valido = "0123456789";
    for (var i = 0; i <= strCadena.length - 1; i++) {
        if (valido.indexOf(strCadena.substring(i, i + 1), 0) == -1) {
            strCadenaFinal = strCadenaFinal.replace(strCadena.substring(i, i + 1), "");
        }
    }

    $(this).val(strCadenaFinal);
    return true;
});
*/
$(document).on("paste", '.solonumeroPaste', function (evt) {
    var regex = new RegExp("^([0-9]+)$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});

$(document).on("keypress", '.numerodecimal', function(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (!((charCode == 8 || charCode == 46) || (charCode >= 48 && charCode <= 57))) return false;
    if ($(this).val().indexOf(".") >= '1') {
        var parts = $(this).val().split('.');
        if (parts[1].length >= 2 && (charCode != 8 && charCode != 46)) return false;
    }
    return true;
});


$(document).on("keypress", '.alfabeticoCompleto', function (evt) {
    var regex = new RegExp("^[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ_'-/. ]+$");
    var str = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
    if (regex.test(str)) {
        return true;
    }
    evt.preventDefault();
    return false;
});

$(document).on("keyup", '.alfabeticoCompleto', function (evt) {

    var strCadena = new String($(this).val());
    if (strCadena == "") return true;

    var strCadenaFinal = strCadena;
    var valido = "ABCDEFGHIJKLMNÑOPQRSTUVWYXZabcdefghijklmnñopqrstuvwxyzñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ' ";
    for (var i = 0; i <= strCadena.length - 1; i++) {
        if (valido.indexOf(strCadena.substring(i, i + 1), 0) == -1) {
            strCadenaFinal = strCadenaFinal.replace(strCadena.substring(i, i + 1), "");
        }
    }

    $(this).val(strCadenaFinal);
    return true;
});

function FP_ValidaAlfaNumericoCP(evt, obj) {

    var strCadena = new String(obj.value);
    if (strCadena == "")
        return true;

    var strCadenaFinal = strCadena;
    var valido = "ABCDEFGHIJKLMNÑOPQRSTUVWYXZabcdefghijklmnñopqrstuvwxyzáéíóú1234567890ÁÉÍÓÚ ";
    for (var i = 0; i <= strCadena.length - 1; i++) {
        if (valido.indexOf(strCadena.substring(i, i + 1), 0) == -1) {
            strCadenaFinal = strCadenaFinal.replace(strCadena.substring(i, i + 1), "");
        }
    }
    obj.value = strCadenaFinal;
    return true;
}

$(document).on('keypress', '.decimal', function (e) {
    var val = /^[0-9\b\d.]+$/.test(e.key);
    if (!val) {
        return false;
    }
});

/*******************************************************
*Le quita el formato #,###,### MTS
*********************************************************/
function fmtoInv(val) {
    val = val + "";
    var cadena1 = val.split(",");
    if (cadena1.length == 1)//No tiene ,
        if (isNaN(val))
            return "0.00"
        else
            return val
    else {

        var fmto = "";
        for (i = 0; i < cadena1.length; i++)
            fmto += cadena1[i];

        if (isNaN(fmto))
            return "0.00";
        else
            return fmto;

    }
}

/*******************************************************
*Pone a formato #,###,###   MTS
*********************************************************/
function fmtoNum(val) {
    var negativo = false, v = "";
    val = val + "";
    if (val.charAt(0) == '-') {
        for (i = 1; i < val.length; i++)
            v += val.charAt(i);
        val = v;
        negativo = true;
    }
    var cadena = val.split(".");
    var num = cadena[0];
    var fmto = "";
    var ind = 1;
    for (i = num.length - 1; i >= 0; i--) {
        fmto = num.charAt(i) + fmto;
        if (ind % 3 == 0 && ind != 3 && ind != num.length)
            fmto = "," + fmto;
        else if (ind == 3 && ind != num.length)
            fmto = "," + fmto;
        ind++;
    }

    var signo = negativo == true ? "-" : "";
    if (typeof (cadena[1]) != "undefined") {//si no es undefined
        return signo + fmto + "." + cadena[1];
    } else
        return signo + fmto;
}

/****************************************************************
*Funcion que te permite redondear un numero a dos decimales MTS
*****************************************************************/
function redondear(val) {
    var negativo = false, v = "";
    val = val + "";
    if (val.charAt(0) == '-') {
        for (i = 1; i < val.length; i++)
            v += val.charAt(i);
        val = v;
        negativo = true;
    }

    var whole = "" + Math.round(val * Math.pow(10, 2));
    var decPoint = whole.length - 2;
    if (eval(decPoint) < 0) {
        result = "0.0" + whole;
    } else if (decPoint != 0) {
        result = whole.substring(0, decPoint);
        result += ".";
        result += whole.substring(decPoint, whole.length);
    } else
        result = "0." + whole.substring(decPoint, whole.length);
    if (result == ".0")
        result = "0.00";

    var signo = negativo == true ? "-" : "";

    return signo + result;
}


/* --------- Manejo de campos de telefonos para limitar la longitud de ingreso --------- */
/* Ver ejemplo en vista SolicitudAfiliacionTrabajador.cshtml (ver clases, data-) */
$(document).on('keypress', '.telefono-fijo', function() {
    var val = $(this).val();

    if (val.length >= 7) {
        return false;
    }
});

$(document).on('keypress', '.telefono-movil', function() {
    var val = $(this).val();

    if (val.length >= 9) {
        return false;
    }
});

//Validar Números Decimales
function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

//retorna un numero o un punto, si el tamaño del length es igual a nEntero y no existe el punto "." se asigna un punto automaticamente
//cumple con el número de decimales nDecimal, solo serán ese número de decimales
function SoloNumerosDecimales3(e, valInicial, nEntero, nDecimal) {
    var obj = e.srcElement || e.target;
    var tecla_codigo = (document.all) ? e.keyCode : e.which;
    var tecla_valor = String.fromCharCode(tecla_codigo);
    var patron2 = /[\d.]/;
    var control = (tecla_codigo === 46 && (/[.]/).test(obj.value)) ? false : true;
    var existePto = (/[.]/).test(obj.value);

    //el tab
    if (tecla_codigo === 8)
        return true;

    if (valInicial !== obj.value) {
        var TControl = obj.value.length;
        if (existePto === false && tecla_codigo !== 46) {
            if (TControl === nEntero) {
                obj.value = obj.value + ".";
            }
        }

        if (existePto === true) {
            var subVal = obj.value.substring(obj.value.indexOf(".") + 1, obj.value.length);

            if (subVal.length > 1) {
                return false;
            }
        }

        return patron2.test(tecla_valor) && control;
    } else {
        if (valInicial === obj.value) {
            obj.value = '';
        }
        return patron2.test(tecla_valor) && control;
    }
}


/* ---------Validacion de Ruc------------*/
function validacionRUC(ruc) {

    if (ruc.length != 11) {
        return false;
    }

    var di = ruc.split("");

    var sumRuc = eval(di[0]) * 5 + eval(di[1]) * 4 + eval(di[2]) * 3 + eval(di[3]) * 2 + eval(di[4]) * 7 + eval(di[5]) * 6 + eval(di[6]) * 5 +
        eval(di[7]) * 4 + eval(di[8]) * 3 + eval(di[9]) * 2;
    var ultimoDigitoRuc = eval(di[10]);

    var enteroRuc = parseInt(sumRuc / 11);
    var enteroRuc11 = enteroRuc * 11;
    var difSumRucEnteroRuc11 = sumRuc - enteroRuc11;
    var dif11DifSumRucEnteroRuc11 = 11 - difSumRucEnteroRuc11;

    if (dif11DifSumRucEnteroRuc11 == 10) {
        digitoVerificador = 0;
    } else if (dif11DifSumRucEnteroRuc11 == 11) {
        digitoVerificador = 1;
    } else {
        digitoVerificador = dif11DifSumRucEnteroRuc11;
    }

    if (digitoVerificador == ultimoDigitoRuc) {
        return true;

    } else {
        return false;
    }

}

/* ---------Validacion de Ruc------------*/
function validacionCUSPP(CUSPP) {
    if (CUSPP == "") {
        return false;
    }
    if (CUSPP.length != 12) {
        return false;
    }
    if (!isInteger(CUSPP.substring(0, 6))) {
        return false;
    }
    if (!isInteger(CUSPP.substring(11, 12))) {
        return false;
    }
    return true;

}

function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

/* ---------------Validacion Fechas -------------*/
function parseDMY(value) {
    var date = value.split("/");
    var d = parseInt(date[0], 10),
        m = parseInt(date[1], 10),
        y = parseInt(date[2], 10);
    return new Date(y, m - 1, d);
}

/*Formatea un decimal al formato #,###.## */
function formatearDecimales(nStr) {
    nStr += '';
    nroNotificaciones = nStr.split('.');
    x1 = nroNotificaciones[0];
    x2 = nroNotificaciones.length > 1 ? '.' + (nroNotificaciones[1]).padStart(2, '0') : '.00';

    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


function valEmail(valor) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(valor);
}


function validarCorreoElectronico(valor, atributo) {

    if (valor.trim().length == 0) {

        return "El " + atributo + " es obligatorio.\n";
    } else if (!valEmail(valor.trim())) {

        return "El " + atributo + " es incorrecto.\n";
    } else if (valor.trim().indexOf(" ") != -1) {

        return "El " + atributo + " no puede contener espacios.\n";
    }

    var mensaje = emailCheckBolean(valor.trim(), atributo);

    return mensaje;
}

//valida Email pero retornando una boolean
function emailCheckBolean(emailStr, atributo) {
    /* Verificar si el email tiene el formato user@dominio. */
    var emailPat = /^(.+)@(.+)$/

    /* Verificar la existencia de caracteres. ( ) < > @ , ; : \ " . [ ] */
    var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"

    /* Verifica los caracteres que son válidos en una dirección de email */
    var validChars = "\[^\\s" + specialChars + "\]"

    var quotedUser = "(\"[^\"]*\")"

    /* Verifica si la dirección de email está representada con una dirección IP Válida */
    var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/

    /* Verificar caracteres inválidos */
    var atom = validChars + '+'
    var word = "(" + atom + "|" + quotedUser + ")"
    var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
    /*domain, as opposed to ipDomainPat, shown above. */
    var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")


    var matchArray = emailStr.match(emailPat)
    //alert("matchArray"+matchArray);

    if (matchArray == null) {
        return "-El " + atributo + " es inválida (Debe contener '@' y punto).\n";

    }

    var user = matchArray[1]
    var domain = matchArray[2]

    // Si el user "user" es valido 
    if (user.match(userPat) == null) {
        // Si no
        return "-El Nombre de cuenta del " + atributo + " es inválido.\n";
    }

    /* Si la dirección IP es válida */
    var IPArray = domain.match(ipDomainPat)
    if (IPArray != null) {
        for (var i = 1; i <= 4; i++) {
            if (IPArray[i] > 255) {
                return "-La Dirección ip del " + atributo + " es inválida.\n";
            }
        }
        //return "";	
    }

    var domainArray = domain.match(domainPat)
    if (domainArray == null) {
        return "-El Dominio del " + atributo + " es inválido.\n";
    }

    var atomPat = new RegExp(atom, "g")
    var domArr = domain.match(atomPat)
    var len = domArr.length
    if (domArr[domArr.length - 1].length < 2 ||
        domArr[domArr.length - 1].length > 3) {
        return "-La Dirección del " + atributo + " es inválida.\n";
    }

    if (len < 2) {
        return "-La Dirección del " + atributo + " es inválida.\n";
    }

    // La dirección de email ingresada es Válida
    return "";
}


//acepta numeros, letras, apostrofes('), guiones(-), slash(/) y punto(.) .
function alphaNumericSpace2(val) {
    val = val.toUpperCase();
    var regex = /^[0-9A-Za-z\s.'/Ñ-]+$/;
    if (regex.test(val)) {
        return true;
    } else {
        return false;
    }
}

function alphaNumericSpace(val) {
    val = val.toUpperCase();
    var regex = /^[0-9A-Za-z\sÑ]+$/;
    if (regex.test(val)) {
        return true;
    } else {
        return false;
    }
}

function alphanumericSpaceName(val) {
    val = val.toUpperCase();
    var regex = new RegExp("^[a-zA-ZñÑäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ_'-/. ]+$");
    if (regex.test(val)) {
        return true;
    } else {
        return false;
    }
}

$(document).on("keypress keyup blur", '.allowNumericWithDecimal', function (event) {
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

$(document).on('blur', 'input.allowNumericWithDecimal', function (event) {
    var texto = $(this).val().trim();
    if (texto == '' || isNaN(texto))
        $(this).val('0.00');
    else
        $(this).val(parseFloat(texto).toFixed(2));
});


$(document).on("keypress keyup", '.allowNumericWithDecimal8', function (event) {
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

$(document).on('blur', 'input.allowNumericWithDecimal8', function (event) {
    var texto = $(this).val().trim();
    if (texto == '' || isNaN(texto))
        $(this).val('0.00000000');
    else
        $(this).val(parseFloat(texto).toFixed(8));
});



function generarComunicado(comunicadoId, title, body, width, heigth) { //, strSubmitFunc, btnText) {

    var newModal = '<div class="modal fade" id="winComunicado_' + comunicadoId + '" tabindex="-1" role="dialog" aria-hidden="true" >' +
        '<div class="modal-dialog" role="document">' +
        '<div class="modal-content" style="width: ' + width + 'px !important; height: ' + heigth + 'px !important;" >' +
        '<div class="modal-header p-1">' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true" aria-label="Close" style="color:#38709e; margin-right:1rem; margin-top:0.2rem;">' +
        '<span aria-hidden="true">&times;</span>' +
        '</button>' +
        '</div>' +
        '<div class="modal-body p-2">' + body +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    $(newModal).appendTo('body');
    $('#winComunicado_' + comunicadoId).draggable({
        handle: '.modal-dialog'
    }).modal({
        backdrop: 'static',
        keyboard: false
    });
}

function hideModal() {
    // Using a very general selector - this is because $('#modalDiv').hide
    // will remove the modal window but not the mask
    $('.modal.in').modal('hide');
}


function GetFromViewState(searchType, myForm, myFunction) {
    if (document.getElementById("ActualGuid") !== null && document.getElementById("ActualGuid") !== undefined && $(document.getElementById("ActualGuid")).val() !== "") {
        if (searchType != null && myForm != null) {
            var inputrb = $(myForm + " input[value=" + searchType + "]");
            establecerTipoBusqueda(inputrb, false);
        }

        if (myFunction != null)
            myFunction();
        else if (myForm != null)
            $(myForm).submit();
    }
}

function unescape(s) {
    return s.replace(/&amp;/g, "&")
        .replace(/&lt;/g, "<")
        .replace(/&gt;/g, ">")
        .replace(/&#39;/g, "'")
        .replace(/&quot;/g, '"');
}

function esNumero(numero) {
    if (!/^([0-9])*$/.test(numero))
        return false;
    return true;
}

function getLastZIndex() {
    var maxZ = Math.max.apply(null, $.map($('body > *'), function(e, n) {
        return parseInt($(e).css('z-index')) || 1;
    }));
    return maxZ;
}

function ayuda(url, altura) {
    window.open(url, null, "height=" + altura + ",width=800,status=no,scrollbars=yes,toolbar=no,menubar=no,location=no");
}


//====================================Afiliado==================================

var optionTemplate = '<option value="{0}">{1}</option>';
var optionTemplateEmpty = '<option selected value="">Seleccione</option>';

$(function () {
    $('[data-toggle="popover"]').popover({
        container: 'body',
        html: true
    });
});

//====================================Modal Timeout==================================
! function (a) {
    "use strict";
    a.sessionTimeout = function (b) {
        function c() { n || (a.ajax({ type: i.ajaxType, url: i.keepAliveUrl, data: i.ajaxData }), n = !0, setTimeout(function () { n = !1 }, i.keepAliveInterval)) }

        function d() {
            clearTimeout(g), (i.countdownMessage || i.countdownBar) && f("session", !0), "function" == typeof i.onStart && i.onStart(i), i.keepAlive && c(), g = setTimeout(function () {
                "function" != typeof i.onWarn ? a("#session-timeout-dialog").modal("show") : i.onWarn(i), e()
            }, i.warnAfter)
        }

        function e() {
            clearTimeout(g), a("#session-timeout-dialog").hasClass("in") || !i.countdownMessage && !i.countdownBar || f("dialog", !0), g = setTimeout(function () {
                "function" != typeof i.onRedir ? window.location = i.redirUrl : i.onRedir(i)
            }, i.redirAfter - i.warnAfter)
        }

        function f(b, c) {
            clearTimeout(j.timer), "dialog" === b && c ? j.timeLeft = Math.floor((i.redirAfter - i.warnAfter) / 1e3) : "session" === b && c && (j.timeLeft = Math.floor(i.redirAfter / 1e3)), i.countdownBar && "dialog" === b ? j.percentLeft = Math.floor(j.timeLeft / ((i.redirAfter - i.warnAfter) / 1e3) * 100) : i.countdownBar && "session" === b && (j.percentLeft = Math.floor(j.timeLeft / (i.redirAfter / 1e3) * 100));
            var d = a(".countdown-holder"), e = j.timeLeft >= 0 ? j.timeLeft : 0;
            if (i.countdownSmart) {
                var g = Math.floor(e / 60), h = e % 60, k = g > 0 ? g + "m" : "";
                k.length > 0 && (k += " "), k += h + "s", d.text(k)
            } else d.text(e + "s");
            i.countdownBar && a(".countdown-bar").css("width", j.percentLeft + "%"), j.timeLeft = j.timeLeft - 1, j.timer = setTimeout(function () { f(b) }, 1e3)
        }

        function createbutton() {
            var btn = document.createElement("button");
            btn.setAttribute("id", "btnresetsession");
            btn.setAttribute("style", "display:none");
            btn.innerHTML = '';
            btn.addEventListener('click', function () { d() });
            document.body.appendChild(btn);
        }

        var g, h = {
            title: "Mensaje del Sistema",
            message: "Tu sesión va a expirar en...",
            logoutButton: "Logout",
            keepAliveButton: "Seguir Conectado",
            keepAliveUrl: "//",
            ajaxType: "POST",
            ajaxData: "",
            redirUrl: "/timeout",
            logoutUrl: "/logout",
            warnAfter: 9e5,
            redirAfter: 12e5,
            keepAliveInterval: 5e3,
            keepAlive: !0,
            ignoreUserActivity: !1,
            onStart: !1,
            onWarn: !1,
            onRedir: !1,
            countdownMessage: !1,
            countdownBar: !1,
            countdownSmart: !1
        },
            i = h, j = {};
        if (b && (i = a.extend(h, b)), i.warnAfter >= i.redirAfter)
            return console.error('El complemento Bootstrap-session-timeout está mal configurado. La opción "redirAfter" debe ser igual o mayor que "warnAfter".'), !1;
        if ("function" != typeof i.onWarn) {
            var k = i.countdownMessage ? "<p>" + i.countdownMessage.replace(/{timer}/g, '<span class="countdown-holder"></span>') + "</p>" : "",
                l = i.countdownBar ? '<div class="progress"><div class="progress-bar progress-bar-striped countdown-bar active" role="progressbar" style="min-width: 15px; width: 100%;"><span class="countdown-holder"></span></div></div>' : "";
            a("body").append('<div class="modal fade" id="session-timeout-dialog" data-bs-backdrop="static" tabindex="-1" role="dialog" data-keyboard="false" data-backdrop="static"><div class="modal-dialog"><div class="modal-content afp-modal-border afp-modal-border-warning">'
            + '<div class="modal-header afp-modal-header afp-modal-header-warning"><h4 class="modal-title">' + i.title + '</h4></div><div class="modal-body"><p>' + i.message + "</p>" + k + "" + l + '</div><div class="modal-footer"><button id="session-timeout-dialog-logout" type="button" class="btn btn-danger">' + i.logoutButton
                + '</button><button id="session-timeout-dialog-keepalive" type="button" class="btn btn-primary" data-bs-dismiss="modal" onclick="sessionKeepAlive();">' + i.keepAliveButton + "</button></div></div></div></div>"), a("#session-timeout-dialog-logout").on("click", function () { window.location = i.logoutUrl }),
            a("#session-timeout-dialog").on("hide.bs.modal", function () { d() }),
            a(document.forms).submit(function () { d() })
        }
        if (!i.ignoreUserActivity) { var m = [-1, -1]; }
        var n = !1;
        d();
        createbutton();
    }
}(jQuery);

