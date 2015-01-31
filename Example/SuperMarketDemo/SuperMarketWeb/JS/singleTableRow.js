//*******************************************
//表格单选选择脚本:邓太华 2005.11.19
//注意脚本文件需要保存为系统定义的编码，比如此处使用配置文件的utf-8,否则可能有错
//修改：邓太华 2008.2.26 增加选项按钮功能，提示选择。
//*******************************************

var myselected=null;              //当前选择的行
var mySelectedValue=null;         //单选的结果值
var oldclassName="None";
var oldSelectedClassName=null;
var cssRowSelected="";            //在调用程序中指定
var cssRowMouseMove="";           //在调用程序中指定
//如果只是单击表格中的链接则不选择，在链接的事件中加入：onclick="ClickHyperLink=true"
var ClickHyperLink=false;

//清除所有选择的行
function ClearAll()
{
	var checkObj=document.getElementsByName("CID");//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
		checkObj[i].checked=false;
}

//鼠标悬浮
function mymove(obj)
{
	if(oldclassName=="None") oldclassName=obj.className;
	if(obj.className!=cssRowSelected)
		obj.className=cssRowMouseMove;
	if(typeof(RowMoveOn)=="function")
		RowMoveOn(obj);
}
//鼠标离开
function myout(obj)
{
	if(obj.className!=cssRowSelected)
		obj.className=oldclassName;//'Uout';
	oldclassName="None";	
	if(typeof(RowMoveOut)=="function")
		RowMoveOut(obj);
}

//单击一行
function myclick(obj,selectValue)
{
	if(ClickHyperLink)
	{
	  ClickHyperLink=false;
	  return;
	}
	//var hiobj=document.getElementById ("SHValue");//必须在页面中指定该隐藏字段
	var hiobj=document.all.item("SHValue");
	
	if(obj.className!=cssRowSelected)
	{
		if(myselected!=null) myselected.className=oldSelectedClassName;//'Uout';
		myselected=obj;
		mySelectedValue=selectValue;
		oldSelectedClassName=oldclassName;//obj.className;
		obj.className=cssRowSelected;
		hiobj.value=mySelectedValue;
		SetCheckBox(selectValue,true);
	}
	else
	{
		//已经被选中，取消
		obj.className=oldSelectedClassName;
		oldclassName=oldSelectedClassName;
		myselected=null;
		mySelectedValue=null;
		hiobj.value="";
		SetCheckBox(selectValue,false);
	}
	
}

function SetCheckBox(checkValue,IsChecked)
{
	var checkObj=document.getElementsByName("CID");//要求选择框的名字为CID
	for(var i=0;i<checkObj.length;i++)
	{
		if(checkObj[i].value==checkValue && checkObj[i].value!="")
		{
			checkObj[i].checked=IsChecked;
			return;
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
//////////////////////////////////
		function checkSelect()
		{
			if(mySelectedValue==null)
			{
				alert("请先选择一条记录！");
				return false;
			}
			return true;
		}
		
		function checkDelete()
		{
			if(checkSelect())
			{
				return confirm("确认删除该记录吗？");
			}
			return false;
		}

