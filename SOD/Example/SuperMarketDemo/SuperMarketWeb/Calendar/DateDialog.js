function MyModalDialog(src,obj)
{
var DateHeight=280;
var DateWidth=310;
//alert(DateHeight);
var top= (screen.height/2 - DateHeight/2) ;
var left= (screen.width/2 - DateWidth/2);
return window.showModalDialog(src,obj,'dialogHeight='+DateHeight+'px;dialogWidth='+DateWidth+'px;status=no;toolbar=no;menubar=no;location=no;');
}
