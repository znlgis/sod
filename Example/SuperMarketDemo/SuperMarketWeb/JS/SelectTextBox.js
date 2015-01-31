/*
-----“选择输入”脚本----------------
功能：自动在当前输入的文本框之后插入可以选择的文本，单击选择，文本写入当前文本框中。
      单击除文本选择框之外的任何地方则关闭选择框。
使用方法：
      如下的方式
      <input type="text" name="T2" size="20"  value="xx"  onclick="CreateDiv(this,50,'111','222','333','444')" onblur="DeleteDiv()" >
      
by bluedoctor,http://www.pwmis.cn 2005.6.6
改写：邓太华 dth1977@sohu.com 2006.6.23
      修改版本主要解决了表单中重复名称的表单元素无法选择值的问题。
-------------------------------------
*/
var LastSelectLayer=null;     //最后选择的层对象
var OnSelectLayer=false;      //标记鼠标在选择框上
var oldSelectTextBoxValue=""; //单击文本框时候文本框中的值
var CurrSelectTextBox=null;
/*
函数 CreateDiv： 创建“选择输入”文本
参数 obj：目标文本框
调用方法：
如果要选择项 "xxxx","yyyy"，那么在文本框的 onclick 中如下调用：
onclick="CreateDiv(this,50,'xxxx','yyyy')"

*/
function CreateDiv(obj,width)
{
//只读选择文本无效
if(obj.readOnly) return;
/*
swhere:指定插入html标签语句的地方，有四种值可以用： 
1.beforeBegin:插入到标签开始前 
2.afterBegin:插入到标签开始标记后 
3.beforeEnd:插入到标签结束标记前 
4.afterEnd:插入到标签结束标记后 
*/

//如果原来的层存在，那么隐藏它
if(LastSelectLayer!=null)
  LastSelectLayer.style.display='none';

var objText=obj.name ;
oldSelectTextBoxValue=obj.value; //定位重复的名字的文本框
CurrSelectTextBox=obj;
var CurrTop=getoffset(obj)[0]; //取得控件当前的顶部位置

var newLayerName="mySelectLayer"+objText;
LastSelectLayer=document.getElementById(newLayerName);
if(LastSelectLayer==null)
{
  var objDiv='<div style="position: absolute; width: 10px; height: 10px; background-color:#808080 ;border:1px solid #000000;z-index: 1" id="'+newLayerName+'" onclick="CloseSelectDiv()" onmouseover="OnSelectLayer=true">';
  //根据参数的长度构建选择项目
  objDiv+='<table border="0" width="98%" bgcolor="#FFFFFF">'
  for(var i=2;i<arguments.length;i++)
      //objDiv+='<tr><td><a href="#" onclick="'+objText+'.value =this.innerText;" title="请选择">'+arguments[i]+'</a></td></tr>';
      objDiv+='<tr><td><a href="#" onclick="SelectMyText(\''+objText+'\',this)" title="请选择">'+arguments[i]+'</a></td></tr>';

  objDiv+='<tr><td align="center"><a href="#" onclick="CloseSelectDiv()" title="关闭">关闭</a></td></tr></table></div>';
  obj.parentElement.insertAdjacentHTML("beforeEnd",objDiv);
  LastSelectLayer=document.getElementById(newLayerName);
  LastSelectLayer.style.width=width;
}
else
{
  LastSelectLayer.style.display="";
}
LastSelectLayer.style.top=CurrTop;

}//end function

/*
函数 SelectMyText
功能：设置当前选择文本框的值
参数：
    objName：选择文本框的名字
    selecter：单击的链接对象
*/
function SelectMyText(objName,selecter)
{
var obj1=document.forms[0].item(objName);
var length=obj1.length;
var findObj=null;
var index=0;

/*
if(length>1)
{
  //定位重复的名字的文本框，对于重名的文本框，只能根据当前的不同值来区分。
  //alert("findObj");
  for(var i=length-1;i>=0;i--)
  {
    if(obj1[i].value==oldSelectTextBoxValue)
    {
      obj1[i].value=selecter.innerText;
      findObj=obj1[i];
      index=i;
      break;
    }
  }
}
else
 {
   //alert(selecter.innerText);
   obj1.value=selecter.innerText;
   findObj=obj1;
 }
//如果在调用页面定了选择值后的处理函数 OnSelectMyText 那么调用该函数
//下面语句可以模拟发起事件

if(typeof(OnSelectMyText)=="function")
 {
  //alert(findObj.value);
  OnSelectMyText(findObj,index);
 }
*/

if(length>1)
{
  //定位重复的名字的文本框，取得当前索引。
  //alert("findObj");
  for(var i=0;i<length; i++)
  {
    if(obj1[i]==CurrSelectTextBox)
    {
      //obj1[i].value=selecter.innerText;
      findObj=obj1[i];
      index=i;
      //alert("index="+index);
      break;
    }
  }
}
else
 {
   //alert(selecter.innerText);
   //obj1.value=selecter.innerText;
   findObj=CurrSelectTextBox;
 }

CurrSelectTextBox.value=selecter.innerText;

if(typeof(OnSelectMyText)=="function")
 {
  //alert(findObj.value);
  OnSelectMyText(findObj,index);
 }

}//end function

//关闭选择框
function CloseSelectDiv()
{
if(LastSelectLayer!=null)
{
   LastSelectLayer.style.display="none";
   OnSelectLayer=false;
}

}

//在文本框失去焦点的时候如果没有意图要选择输入，那么关闭选择框
function DeleteDiv()
{
if(!OnSelectLayer)
  CloseSelectDiv();
}
//----选择输入脚本 结束 --

function getoffset(e) 
{  
 var t=e.offsetTop;  
 var l=e.offsetLeft;  
 while(e=e.offsetParent) 
 {  
  t+=e.offsetTop;  
  l+=e.offsetLeft;  
 }  
 var rec = new Array(1); 
 rec[0]  = t; 
 rec[1] = l; 
 return rec 
}  
