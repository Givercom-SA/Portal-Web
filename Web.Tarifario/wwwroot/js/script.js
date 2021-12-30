function muestra_oculta(id){
    if (document.getElementById){ 
    var el = document.getElementById(id);  
    el.style.display = (el.style.display == 'none') ? 'block' : 'none' ; 
    }
}