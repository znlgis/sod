//*******************************************
//表格选择脚本:邓太华 2006.3.6
//功能：在表格的任意一行单击鼠标选择一行，可以选择多行。

//     要求在某一列中有复选框控件 <input type='checkbox' name='CID' value='value1'>，

//     名字必须为 'CID' ，每一个复选框控件的值都必须不同，建议将数据库表的主键值绑定到它上面。

//版本记录：

//Ver 1.2 该版本可以记录上次选择的项
//Ver 1.2.1 在控件不刷新的时候保存状态

//Ver 1.2.2 记录最后选中的值及当前页选中的总记录数
//Ver 1.2.3 修正全选后选择数量统计的问题
//Ver 1.2.4 增加列表项目绑定值为 "" 的时候选择功能失效 的功能，可以在程序运行时候指定那些项目可以选择
//Ver 1.2.5 修正当列表项目有不可选项时候的编辑/删除控制问题

//注意脚本文件需要保存为系统定义的编码，比如此处使用配置文件的utf-8,否则可能有错
//如果在回退操作的时候不希望保留上一次的选择（其实该选择已经无效，但是复选框确实选中的），请使用 <body  onload="ClearAll()">
//有问题请联系 Email:dth1977@sohu.com MSN:bluedoctors@msn.com
//*******************************************
var oldclassName="None";
var oldSelectedClassName=null;
var cssRowSelected="Uselected";  //选中一行时候的样式名

var cssRowMouseMove="Umove";     //鼠标移动时候行的样式名
var mySelectedList=new Array();  //选中行队列

var cssSelectedList=new Array(); //选中行样式队列

//如果只是单击表格中的链接则不选择，在链接的事件中加入：onclick="ClickHyperLink=true"
var ClickHyperLink=false;

var ClientSelectedCount=0;       //客户端选中的数量

var LastSelectedValue=null;      //最后选中的值
function GetCheckBoxList()
{
	return document.getElementsByName("CID");
}

function GetCheckAllBox()
{
	return document.getElementsByName("CheckAll");
}

//鼠标悬浮时候的样式处理
function mymove(obj)
{
	if(oldclassName=="None") oldclassName=obj.className;
	if(obj.className!=cssRowSelected)
		obj.className=cssRowMouseMove;
}

//鼠标离开时候的样式处理
function myout(obj)
{
	if(obj.className!=cssRowSelected)
		obj.className=oldclassName;//'Uout';
	oldclassName="None";	
}

//单击一行

function myclick(obj,checkValue)
{
	if(ClickHyperLink)
	{
	  ClickHyperLink=false;
	  return;
	}
	//如果值为空，那么不可选择
	if(checkValue=="")return;
	var SHValue=document.all.item ("SHValue");//标记客户端发生了选择事件
	SHValue.value="-1";
	
	if(obj.className!=cssRowSelected)
	{
		//myselected=obj;
		oldSelectedClassName=oldclassName;//obj.className;
		obj.className=cssRowSelected;
		if(!objExists(obj))
			objAdd(obj,oldSelectedClassName);
			
		SetCheckBox(checkValue,true);
		LastSelectedValue=checkValue;
		ClientSelectedCount++;		
	}
	else
	{
		//myselected=null;
		objRemove(obj);
		oldclassName=obj.className;
		SetCheckBox(checkValue,false);
		LastSelectedValue=null;
		if(ClientSelectedCount>0)ClientSelectedCount--;
	}
	//alert("ClientSelectedCount="+ClientSelectedCount);
}

//设置某一个值为checkValue 的复选框的状态

function SetCheckBox(checkValue,IsChecked)
{
	var checkObj=GetCheckBoxList();//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
	{
		if(checkObj[i].value==checkValue && checkObj[i].value!="")
		{
			checkObj[i].checked=IsChecked;
			return;
		}
	}
}

//设定 全选/取消全选
//邓太华 2005.5.24 修改当列表有不可选的情况下的全选处理问题
function CheckedAll()
{
	var checkAll=GetCheckAllBox();
	var flag=checkAll[0].checked;
	var checkObj=GetCheckBoxList();//要求选择框的名字为CID
	ClientSelectedCount=0;
	for(var i=0;i<checkObj.length;i++)
	{
		if(checkObj[i].value!="") 
		{
			checkObj[i].checked=flag;
			if(flag)
				ClientSelectedCount++;
		}
	}
	if(!flag)
		ClientSelectedCount=0;

}

//清除所有选择的行
function ClearAll()
{
	var checkObj=GetCheckBoxList();//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
		checkObj[i].checked=false;
	ClientSelectedCount=0;

}

//重新计算当前实际选择的项目数量

function RealSelectedCount()
{
	ClientSelectedCount=0;
	var checkObj=GetCheckBoxList();//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
		if(checkObj[i].checked)
			ClientSelectedCount++;

}

//仅仅选择一项时候的值，用于编辑状态

function OnlyOneSelecedValue()
{
	if(ClientSelectedCount>1) return null;
	var checkObj=GetCheckBoxList();//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
		if(checkObj[i].checked)
			return checkObj[i].value;
	return null;
}

//判断某行是否在队列中
function objExists(obj)
{
	for(var i=0;i<mySelectedList.length ;i++)
	{
		if(mySelectedList[i]==obj)
		return true;
	}
	return false;
}

//向队列中添加一行

function objAdd(obj,cssName)
{
	var i=0;
	for( i=0;i<mySelectedList.length ;i++)
	{
		if(mySelectedList[i]==null)
		{
			mySelectedList[i]=obj;//利用空域
			cssSelectedList[i]=cssName;
			return ;
		}
	}
	mySelectedList[i]=obj;//插入新的对象
	cssSelectedList[i]=cssName;
	
}

//从队列中移除一行

function objRemove(obj)
{
	for(var i=0;i<mySelectedList.length ;i++)
	{
		if(mySelectedList[i]==obj)
		{
			mySelectedList[i]=null;//在数组中删除对象
			obj.className=cssSelectedList[i];
			return ;
		}
	}
}

//初始化客户端选择
function InitLastSelected(strLastSelected)
{
 //var strLastSelected="3,4,5";
 var arrValues=strLastSelected.split(",");
 for(var i=0;i<arrValues.length;i++)
 SetCheckBox(arrValues[i],true);
}

//设置每一个复选框绑定的值
//2006.04.24 修改，如果绑定值为空，那么禁止使用该复选框
function SetCheckValues()
 {
   //要求必须有 隐藏控件 SelectValueList
   var SelectValueList=document.all.item ("SelectValueList").value;
   var arrSelectValue=SelectValueList.split(",");
   var checkObj=GetCheckBoxList();//要求选择框的名字为CID
   for(var i=0;i<checkObj.length;i++)
   {
	 checkObj[i].value=arrSelectValue[i];
     if(checkObj[i].value=="") 
       checkObj[i].disabled=true;
   }
 }

//编辑时检查

function CheckSelect(obj)
{
  if(ClientSelectedCount==0)
  {
	alert("请选择您要编辑的记录！");
	return false;
  }
  else  if(ClientSelectedCount>1)
  {
  	alert("您选择了"+ClientSelectedCount+"条记录，但是每次只能编辑一条记录，请确定唯一的一条！");
  	return false;
  }
  else
  {
   	var EditID=OnlyOneSelecedValue();
  	obj.href+="?EditID="+EditID;
  	//alert("当前要编辑的地址："+obj.href);
  	return true;
  }
}

function CheckSelect2()
{
  if(ClientSelectedCount==0)
  {
	alert("请选择您要编辑的记录！");
	return false;
  }
  else  if(ClientSelectedCount>1)
  {
  	alert("您选择了"+ClientSelectedCount+"条记录，但是每次只能编辑一条记录，请确定唯一的一条！");
  	return false;
  }
  else
  {
   
  	return true;
  }
}

function DeleteConfirm()
{
	if(ClientSelectedCount==0)
  	{
		alert("请至少选择一条您要操作的记录！");
		return false;
  	}
	return confirm("当前您选择了"+ClientSelectedCount+"条记录，确认当前操作吗？");
}
function DeleteConfirm1()
{
	if(ClientSelectedCount==0)
  	{
		alert("请至少选择一条记录！");
		return false;
  	}
	return true;
}
function GoNextConfirm(bFlag,Msg,objID)
{	
	//alert(bFlag)
	if(bFlag)
	{
		if(confirm(Msg))
		{
			document.getElementById(objID).value = "1";			
		}
		else
		{
			document.getElementById(objID).value = 0;
		}
		
		return true;
	}
	else
		return false;
}

/*
函数： getQueryString
参数： key 
功能： 在客户端获取页面指定参数的值

*/
function getQueryString(key)
{
　　 var value = ""; 
　　 //获取当前文档的URL,为后面分析它做准备

　　 var sURL = window.document.URL;
　　 
　　 //URL中是否包含查询字符串
　　 if (sURL.indexOf("?") > 0)
　　 {
　　	//分解URL,第二的元素为完整的查询字符串
　　　	var arrayParams = sURL.split("?");
　　 
　　	//分解查询字符串

　　	var arrayURLParams = arrayParams[1].split("&");
　　 
　　	//遍历分解后的键值对
　　	for (var i = 0; i < arrayURLParams.length; i++)
　　	{
　　		 //分解一个键值对
　　		var sParam = arrayURLParams[i].split("=");
　　 
　　		if ((sParam[0] == key) && (sParam[1] != ""))
　　		{
　　			//找到匹配的的键,且值不为空
　　			value = sParam[1];
　　 
　　			break;
　　		}
　　	} 
　　 }
　　 return value;
}