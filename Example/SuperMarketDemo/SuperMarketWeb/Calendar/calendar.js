

var gdCtrl = new Object();
var goSelectTag = new Array();
var gcGray = "#cccccc";
var gcToggle = "#cccccc";
var gcSel = "#ccccc0";
var gcSelBorder = "#666660";
var gcToggleBorder = "#666666";
var gcLinkText = '#ffff00';
var gcToggleText = '#00ff00';
var gcBG = "#efefef";
var gcTitleBG="#cccccc";
var gcTitleText="#006600"
var previousObject = null;

var gdCurDate = new Date();
var giYear = gdCurDate.getFullYear();
var giMonth = gdCurDate.getMonth()+1;
var giDay = gdCurDate.getDate();



var gCalMode = "";
var gCalDefDate = "";

var sCell;

var CAL_MODE_NOBLANK = "2";

var giHours = gdCurDate.getHours();

var giMinutes = gdCurDate.getMinutes();

var giSeconds = gdCurDate.getMinutes();

function fClose()
{
window.close();
}
function fClear()
{
window.returnValue = null;
window.close();
}

function fSetDate(iYear, iMonth, iDay, iHours, iMinutes, iSeconds){
  //VicPopCal.style.visibility = "hidden";
  if ((iYear == 0) && (iMonth == 0) && (iDay == 0)){
  	gdCtrl.value = "";
  }else{
  	iMonth = iMonth + 100 + "";
  	iMonth = iMonth.substring(1);
  	iDay   = iDay + 100 + "";
  	iDay   = iDay.substring(1);
  	//alert(IsFullTimeStr.checked);
  	if(IsFullTimeStr.checked)
  	{
  	gdCtrl.value = iYear+"-"+iMonth+"-"+iDay + " " + iHours +":"+iMinutes+":00";//+gdCurDate.getSeconds();
  	}
  	else
  	{
  	gdCtrl.value = iYear+"-"+iMonth+"-"+iDay;
  	}
  }
  
  for (i in goSelectTag)
  	goSelectTag[i].style.visibility = "visible";
  goSelectTag.length = 0;
 
  window.returnValue=gdCtrl.value;
//  
  //window.close();
}

function HiddenDiv()
{
	var i;
  VicPopCal.style.visibility = "hidden";
  for (i in goSelectTag)
  	goSelectTag[i].style.visibility = "visible";
  goSelectTag.length = 0;

}

function fSelHours()
{
giHours = parseInt(tbSelHours.value);
}

function fSelMinutes()
{
giMinutes = parseInt(tbSelMinutes.value);
}

function fSetSelected(aCell){
  var iOffset = 0;
  var iYear = parseInt(tbSelYear.value);
  var iMonth = parseInt(tbSelMonth.value);
 // var iHours = parseInt(tbSelHours.value);
 // var iMinutes = parseInt(tbSelMinutes.value);
  //var iSeconds = date.getSeconds();
  
  //aCell.bgColor = gcBG;
  if(sCell != null)
  {
  sCell.bgColor = gcBG;
  sCell.borderColor = gcBG;
  }
  sCell = aCell;
  aCell.bgColor=gcSel;
  aCell.borderColor=gcSelBorder;
  with (aCell.children["cellText"]){
  	var iDay = parseInt(innerText);
  	if (color==gcGray)
		iOffset = (Victor<10)?-1:1;

	/*** below temp patch by maxiang ***/
	if( color == gcGray ){
		iOffset = (iDay < 15 )?1:-1;
	}
	/*** above temp patch by maxiang ***/

	iMonth += iOffset;
	if (iMonth<1) {
		iYear--;
		iMonth = 12;
	}else if (iMonth>12){
		iYear++;
		iMonth = 1;
	}
  }

  giYear = parseInt(tbSelYear.value);
  
  giMonth = parseInt(tbSelMonth.value);
  
  giDay = iDay;
  
}

function Point(iX, iY){
	this.x = iX;
	this.y = iY;
}

function fBuildCal(iYear, iMonth) {
  var aMonth=new Array();
  for(i=1;i<7;i++)
  	aMonth[i]=new Array(i);
  
  var dCalDate=new Date(iYear, iMonth-1, 1);
  var iDayOfFirst=dCalDate.getDay();
  var iDaysInMonth=new Date(iYear, iMonth, 0).getDate();
  var iOffsetLast=new Date(iYear, iMonth-1, 0).getDate()-iDayOfFirst+1;
  var iDate = 1;
  var iNext = 1;

  for (d = 0; d < 7; d++)
	aMonth[1][d] = (d<iDayOfFirst)?-(iOffsetLast+d):iDate++;
  for (w = 2; w < 7; w++)
  	for (d = 0; d < 7; d++)
		aMonth[w][d] = (iDate<=iDaysInMonth)?iDate++:-(iNext++);
  return aMonth;
}

function fDrawCal(iYear, iMonth, iCellHeight, sDateTextSize) {
  var WeekDay = new Array("日","一","二","三","四","五","六");
  var styleTitleTD = " bgcolor='"+gcTitleBG+"' bordercolor='"+gcTitleBG+"' valign='middle' align='center' height='"+iCellHeight+"' style='font-size:12px; ";
  var styleTD = " bgcolor='"+gcBG+"' bordercolor='"+gcBG+"' valign='middle' align='center' height='"+iCellHeight+"' style='font-size:12px; ";

  with (document) {
	write("<tr>");
	for(i=0; i<7; i++)
		write("<td "+styleTitleTD+" color:"+gcTitleText+"' style='padding:3'><b>" + WeekDay[i] + "</b></td>");
	write("</tr>");

  	for (w = 1; w < 7; w++) {
		write("<tr>");
		for (d = 0; d < 7; d++) {
			write("<td id=calCell "+styleTD+"cursor:hand;' style='padding-left:8;padding-right:8;padding-top:5;padding-bottom:5' onMouseOver='if(this.bgColor!=gcSel){this.bgColor=gcToggle;this.borderColor=gcToggleBorder}' onMouseOut='if(this.bgColor!=gcSel){this.bgColor=gcBG;this.borderColor=gcBG}' onclick='fSetSelected(this)'>");
			write("<font id=cellText face=Verdana><b> </b></font>");
			write("</td>")
		}
		write("</tr>");
	}
  }
}

function fUpdateCal(iYear, iMonth) {
  myMonth = fBuildCal(iYear, iMonth);
  var i = 0;
  for (w = 0; w < 6; w++)
	for (d = 0; d < 7; d++)
		with (cellText[(7*w)+d]) {
			Victor = i++;
			if (myMonth[w+1][d]<0) {
				color = gcGray;
				innerText = -myMonth[w+1][d];
			}else{
				// Modified by maxiang for we need 
				// Saturday displayed in blue font color.
				//color = ((d==0)||(d==6))?"red":"black";
				if( d == 0 ){
					color = "red";
				}else if( d == 6 ){
					color = "red";
				}else{
					color = "black";
				}
				// End of above maxiang
				innerText = myMonth[w+1][d];
			}
		}
}

function fSetYearMon(iYear, iMon){
  tbSelMonth.options[iMon-1].selected = true;
  for (i = 0; i < tbSelYear.length; i++)
	if (tbSelYear.options[i].value == iYear)
		tbSelYear.options[i].selected = true;
  fUpdateCal(iYear, iMon);
}

function fPrevMonth(){
  var iMon = tbSelMonth.value;
  var iYear = tbSelYear.value;
  
  if (--iMon<1) {
	  iMon = 12;
	  iYear--;
  }
  
  fSetYearMon(iYear, iMon);
}

function fNextMonth(){
  var iMon = tbSelMonth.value;
  var iYear = tbSelYear.value;
  
  if (++iMon>12) {
	  iMon = 1;
	  iYear++;
  }
  
  fSetYearMon(iYear, iMon);
}

function fToggleTags(){
  with (document.all.tags("SELECT")){
 	for (i=0; i<length; i++)
 		if ((item(i).Victor!="Won")&&fTagInBound(item(i))){
 			item(i).style.visibility = "hidden";
 			goSelectTag[goSelectTag.length] = item(i);
 		}
  }
}

function fTagInBound(aTag){
  with (VicPopCal.style){
  	var l = parseInt(left);
  	var t = parseInt(top);
  	var r = l+parseInt(width);
  	var b = t+parseInt(height);
	var ptLT = fGetXY(aTag);
	return !((ptLT.x>r)||(ptLT.x+aTag.offsetWidth<l)||(ptLT.y>b)||(ptLT.y+aTag.offsetHeight<t));
  }
}

function fGetXY(aTag){
  var oTmp = aTag;
  var pt = new Point(0,0);
  do {
  	pt.x += oTmp.offsetLeft;
  	pt.y += oTmp.offsetTop;
  	oTmp = oTmp.offsetParent;
  	if (oTmp == null)
  	    break;
  } while(oTmp.tagName!="BODY");
  return pt;
}

// Main: popCtrl is the widget beyond which you want this calendar to appear;
//       dateCtrl is the widget into which you want to put the selected date.
// i.e.: <input type="text" name="dc" style="text-align:center" readonly><INPUT type="button" value="V" onclick="fPopCalendar(dc,dc);return false">
function fPopCalendar(popCtrl, dateCtrl, mode, defDate){
	gCalMode = mode;
	gCalDefDate = defDate;
	
  if (popCtrl == previousObject){
	  	if (VicPopCal.style.visibility == "visible"){
  		//HiddenDiv();
  		return true;
  	}
  	
  }
  previousObject = popCtrl;
  gdCtrl = dateCtrl;
  fSetYearMon(giYear, giMonth); 
  var point = fGetXY(popCtrl);

	if( gCalMode == CAL_MODE_NOBLANK ){
		document.all.CAL_B_BLANK.style.visibility = "hidden";	
	}else{
		document.all.CAL_B_BLANK.style.visibility = "visible";
	}	

  with (VicPopCal.style) {
  	left = point.x;
	top  = point.y+popCtrl.offsetHeight;
	width = VicPopCal.offsetWidth;
	height = VicPopCal.offsetHeight;
	fToggleTags(point); 	
	visibility = 'visible';
  }
}

var gMonths = new Array("1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月");
var gHours = new Array("1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24");

with (document) {
write("<Div id='VicPopCal' style='OVERFLOW:hidden;POSITION:absolute;VISIBILITY:hidden;border:0px ridge;width:100%;height:100%;top:0;left:0;z-index:100;overflow:hidden'>");
write("<table border='0' bgcolor='#3366cc' cellspacing=0 cellpadding=1>");
write("<TR>");
write("<td valign='middle' align='left'>");
write("<SELECT name='tbSelYear' onChange='fUpdateCal(tbSelYear.value, tbSelMonth.value)' Victor='Won'>");
for(i=1930;i<2029;i++)
	write("<OPTION value='"+i+"'>"+i+"年</OPTION>");
write("</SELECT>");
write("&nbsp;<select name='tbSelMonth' onChange='fUpdateCal(tbSelYear.value, tbSelMonth.value)' Victor='Won'>");
for (i=0; i<12; i++)
	write("<option value='"+(i+1)+"'>"+gMonths[i]+"</option>");
write("</SELECT>");
write("&nbsp;&nbsp;<span title='上一月份' onclick='javascript:fPrevMonth()' onmouseover='this.style.color=gcToggleText' onMouseOut='this.style.color=gcLinkText' style='cursor:hand; font-family:webdings; color:"+gcLinkText+"'>3</span>");
write("<span title='下一月份' onclick='javascript:fNextMonth()' onmouseover='this.style.color=gcToggleText' onMouseOut='this.style.color=gcLinkText' style='cursor:hand; font-family:webdings; color:"+gcLinkText+"'>4</span>");

write("&nbsp;<select name='tbSelHours'onChange='fSelHours()' Victor='Won'>");
for (i=0; i<24; i++)
if(gHours[i]==gdCurDate.getHours())
{
	write("<option selected value='"+(i+1)+"'>"+gHours[i]+"点</option>");
	}
	else
	{
	write("<option value='"+(i+1)+"'>"+gHours[i]+"点</option>");
	}
write("</SELECT>");
write("<SELECT name='tbSelMinutes' onChange='fSelMinutes()' Victor='Won'>");
for(i=0;i<60;i++)
if(i==gdCurDate.getMinutes())
{
	write("<OPTION selected value='"+i+"'>"+i+"分</OPTION>");
	}else
	{
	write("<OPTION value='"+i+"'>"+i+"分</OPTION>");
	}
write("</SELECT>");

write("</td>");
write("</TR><TR>");
write("<td align='right'>");
write("<DIV style='background-color:#cccccc'><table width='100%' border='1' cellspacing=1 cellpadding=0>");
fDrawCal(giYear, giMonth, 8, '12');
write("</table></DIV>");
write("</td>");
write("</TR><TR><TD align='center'>");
write("<TABLE width='100%'><TR><TD align='right'>");

write("<INPUT name='IsFullTimeStr' type='checkbox'><span style='color:"+gcLinkText+"; font-size:9pt'>详细时间</span>");

write("<span style='color:"+gcLinkText+"; visibility:visible; cursor:hand; font-size:9pt' onclick='fSetDate(giYear, giMonth, giDay, giHours, giMinutes, giSeconds);fClose();' onMouseOver='this.style.color=gcToggleText' onMouseOut='this.style.color=gcLinkText' title=确定>[ 确定 ]</span>");

write("<span ID=\"CAL_B_BLANK\" style='color:"+gcLinkText+";visibility:visible; cursor:hand; font-size:9pt' onclick='fClear()' onMouseOver='this.style.color=gcToggleText' onMouseOut='this.style.color=gcLinkText' title=清除日期框中的值>[ 清空 ]</span>");

write("<span style='color:"+gcLinkText+";cursor:hand; font-size:9pt' onclick='fSetDate(gdCurDate.getYear(),(gdCurDate.getMonth()+1),gdCurDate.getDate(),gdCurDate.getHours(),gdCurDate.getMinutes(),gdCurDate.getSeconds());fClose();' onMouseOver='this.style.color=gcToggleText' onMouseOut='this.style.color=gcLinkText' title=选择当前日期>  ["+gdCurDate.getFullYear()+"-"+(gdCurDate.getMonth()+1)+"-"+gdCurDate.getDate()+ " " + gdCurDate.getHours()+":"+gdCurDate.getMinutes()+":"+gdCurDate.getSeconds()+" ]</span>");
write("</td></tr></table>");
write("</TD></TR>");
write("</TABLE></Div>");
}
