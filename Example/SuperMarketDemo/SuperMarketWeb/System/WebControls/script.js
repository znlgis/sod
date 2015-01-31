


var DTControl_PopType = "tip";//"{pop,alert,tip}定义提示信息的方式"
var InputOnFocusColor = "#FFFFCC";

var DTControl_bannerAD=new Array();
var DTControl_bannerADlink=new Array();
var DTControl_adNum=0;
if(DTControl_PopType=="tip")
{
//RootSitePath 变量 在Load事件中已经注册
var divStr="<div id='DTControl_TIPDIV' style='position:absolute; z-index:999; left: 237px; top: 134px;display:none' onclick=\"this.style.display='none'\"><table id=\"__01\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"right\" valign=\"top\"><img src=\""
+RootSitePath+"System/WebControls/TIPIMG_01.gif\" width=\"13\" height=\"9\" alt=\"\"></td><td colspan=\"2\" background=\""
+RootSitePath+"System/WebControls/TIPIMG_02.gif\" bgcolor=#ffffff></td><td align=\"left\" valign=\"top\" background=\""
+RootSitePath+"System/WebControls/TIPIMG_04.gif\"></td></tr><tr><td align=\"right\" valign=\"top\" background=\""
+RootSitePath+"System/WebControls/TIPIMG_08.gif\"></td><td colspan=\"2\" rowspan=\"2\"><table width=\"100%\" border=\"0\" bgcolor=\"#FFFFFF\"><tr><td bgcolor=\"#FFFFFF\"><span id='DTControl_TipSpan' style='font-size:12px;color: #990000;font-weight: bold;'></span></td></tr></table></td> <td rowspan=\"2\" background=\""
+RootSitePath+"System/WebControls/TIPIMG_11.gif\"></td> </tr> <tr> <td background=\""
+RootSitePath+"System/WebControls/TIPIMG_08.gif\"></td> </tr><tr><td> <img src=\""
+RootSitePath+"System/WebControls/TIPIMG_12.gif\" width=\"13\" height=\"7\" alt=\"\"></td><td colspan=\"2\" background=\""
+RootSitePath+"System/WebControls/TIPIMG_13.gif\"></td><td><img src=\""
+RootSitePath+"System/WebControls/TIPIMG_15.gif\" width=\"5\" height=\"7\" alt=\"\"></td></tr></table></div>";
document.write(divStr);
}

function DTControl_setTransition(){
  if (document.all){
   document.images.DTControl_bannerADrotator.filters.revealTrans.Transition=Math.floor(Math.random()*23);
   document.images.DTControl_bannerADrotator.filters.revealTrans.apply();
  }
}

function DTControl_playTransition(){
  if (document.all)
  document.images.DTControl_bannerADrotator.filters.revealTrans.play()
}

function DTControl_nextAd(){
  if(DTControl_adNum<DTControl_bannerAD.length-1)DTControl_adNum++ ;
   else DTControl_adNum=0;
  DTControl_setTransition();
  document.images.DTControl_bannerADrotator.src=DTControl_bannerAD[DTControl_adNum];
  DTControl_playTransition();
  theTimer=setTimeout("DTControl_nextAd()", 6000);
}

function DTControl_jump2url(){
  jumpUrl=DTControl_bannerADlink[DTControl_adNum];
  jumpTarget='_self';
  if (jumpUrl != ''){
   if (jumpTarget != '')window.open(jumpUrl,jumpTarget);
   else location.href=jumpUrl;
  }
}



function DTControl_Hide_TIPDIV()
{
  document.all.DTControl_TIPDIV.style.display='none';
}
function DTControl_SetInputBG(Obj)
{
   Obj.style.background = InputOnFocusColor;
  
}

function DTControl_CleInputBG(Obj)
{
  Obj.style.background = '';
}

function DTControlgetoffset(e) 
{  

 var t=e.offsetTop;  
 var l=e.offsetLeft + e.offsetWidth;  
 while(e=e.offsetParent) 
 {  
  t+=e.offsetTop;  
  l+=e.offsetLeft;  
 }  
 var rec = new Array(1); 
 rec[0]  = t; 
 rec[1] = l; 
 return rec;

}  


function DTControl_HideTIPDIV()
{
 document.all.DTControl_TIPDIV.style.display = 'none';

}
function showTIPForTIPSPAN(Message,obj)
{

  var tl = DTControlgetoffset(obj);

  document.all.DTControl_TipSpan.innerHTML = Message;

  var Left = tl[1];
  var Top  = tl[0];
  document.all.DTControl_TIPDIV.style.left = Left;
  document.all.DTControl_TIPDIV.style.top  = Top;
  obj.onkeydown =DTControl_HideTIPDIV;
  document.all.DTControl_TIPDIV.style.display = 'block';
 
}
///Pop模式的标题，信息内容，是Pop模式
function ShowMessage(Message,obj,messageType)
{

  //if(DTControl_PopType=="alert")
  if(messageType=="alert")
  {
   alert(Message);
  }
  else if(messageType=="tip")
  {
  showTIPForTIPSPAN(Message,obj);
  
  }
  return false;
}


//用户自定义正则表达式验证
function isCustomRegular(s,REstring,errMessage,messageType) 
{ 
//debugger;
var patrn=eval('/'+ REstring +'/'); 
if (!patrn.exec(s.value))
	{
	ShowMessage(errMessage,s,messageType);
	s.focus();
	return false;
	}
	return true;
} 
//校验日期//通过测试
function isDateValue(s,errMessage) 
{ 
var patrn=/^[\\d]{4}-[\\d]{2}-[\\d]{2}$/;

if (!patrn.exec(s.value)) 
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false ;
	}
return true 
} 

//校验用户姓名：只能输入字母、数字、下划线 通过测试////////////////
function isTrueName(s,min,max,errMessage) 
{ 
var patrn=eval('/^[a-zA-Z][a-zA-Z0-9_]{' + (min-1) + ',' + (max-1) + '}$/'); 
if (!patrn.exec(s.value))
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false;
	}
	return true;
} 

//字符串检查 通过测试////////////////
function isText(s,min,max,errMessage) 
{ 

var thisvalue = s.value;
var thisLength = thisvalue.length;
if(thisLength<min)
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false;

	}

	if(thisLength>max)
	{
	ShowMessage(errMessage,s);
	
	s.focus();
	return false;

	}

	return true;
} 

//校验密码：只能输入6-20个字母、数字、下划线 通过测试////////////////
function isPasswd(s,min,max,errMessage) 
{ 
var patrn=eval('/^[a-zA-Z][a-zA-Z0-9_]{' + (min-1) + ',' + (max-1) + '}$/'); 

if (!patrn.exec(s)) 
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false;
	}
	return true;
} 




//校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-” //通过测试
function isTel(s,errMessage) 
{ 
//var patrn=/^[+]{0,1}(\d){1,3}[ ]?([-]?(\d){1,12})+$/; 
var patrn=/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/; 
if (!patrn.exec(s.value))
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false ;
	}
return true 
} 



//校验手机号码：必须以数字开头，除数字外，可含有“-” //通过测试
function isMobil(s,errMessage) 
{ 
var patrn=/^[ ]?([-]?((\d)|[ ]){11})+$/;
if (!patrn.exec(s.value)) 
	{
	ShowMessage(errMessage,s);

	s.focus();
	return false ;
	}
return true 
} 




//校验邮政编码 通过测试
function isPostalCode(s,errMessage) 
{ 
var patrn=/^[a-zA-Z0-9 ]{6,6}$/; 
if (!patrn.exec(s.value)) 
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false ;
	}
return true 
} 


//校验数字。对象，最小值，最大值，最多小数点位数，错误信息头
function isNumber(s,min,max,AfterDecimalNumber,errMessage) 
{ 

	var patrn=eval('/^[-0-9]+(.[0-9]{0,' + AfterDecimalNumber + '})?$/');
	var thisvalue = s.value;
	if(thisvalue<min)
	{
	ShowMessage(errMessage,s);
		s.focus();
		return false
	}

	if(thisvalue>max)
	{
	ShowMessage(errMessage,s);
		s.focus();
		return false
	}



if (!patrn.exec(s.value)) 
	{
	ShowMessage(errMessage,s);
	s.focus();
	return false ;
	}
return true 
} 
