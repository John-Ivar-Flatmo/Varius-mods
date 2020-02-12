////////////////////////////////////////////////////////////////////////////////////////
//options
int lcdUpdateDelay = 1; //delays checking for new lcds to improve performance value 1-100
int checkindexs = 100; //how many inventory slots to check for items when equalizing


//options end
////////////////////////////////////////////////////////////////////////////////////////
//variables
int mainRunCount = 100;
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
int transRate = 1;
//variables
////////////////////////////////////////////////////////////////////////////////////////
public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
}
////////////////////////////////////////////////////////////////////////////////////////
public void Main(string argument) {
meths meth = new meths();
mainRunCount++;
////////////////////////////////////////////////////////////////////////////////
//lcd cycle
consoles = new List<IMyTextPanel>();
List<IMyTextPanel> lcds = new List<IMyTextPanel>();
 if(mainRunCount > lcdUpdateDelay){ //get lcds
mainRunCount = 0;
GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
}; //get lcds end
int whil = 0;
while(whil < lcds.Count){  //while
IMyTextPanel lcd = lcds[whil];
string lcdCD = lcd.CustomData;
string lcdCDLC = lcd.CustomData.ToLower();

////////////////////////////////////////////////////////////////////
//consoles init
if(lcdCDLC.Length >= 10){
 if(lcdCDLC.Substring(0,10) == "jifconsole"){
  consoles.Add(lcd);
 };
};
int whil2 = 0;
meth.log(consoles,"Consoles",true,false);
while(consoles.Count > whil2){
IMyTextPanel console = consoles[whil2];
meth.log(consoles,"Init"+console);
whil2++;
};
meth.log(consoles,"Code start");

//consoles init end
////////////////////////////////////////////////////////////////////
//inventory
/////////////////////////////////////
//info
if(lcdCDLC.IndexOf("info") == -1){lcd.WritePublicText(lcd.GetPublicText()+"\n"+"info not specified usage info:info in here: examples info:all: info:Ice");};
if(lcdCDLC.IndexOf("info") != -1){
int aftrinf = lcdCDLC.IndexOf(":",lcdCDLC.IndexOf("info"));
string res = lcdCD.Substring(aftrinf+1,lcdCDLC.IndexOf(":",aftrinf+1)-(aftrinf+1));

/////////////////////////////
//all.all
if(res == "all"){





}; //inventory.all end
/////////////////////////////
//all.info
if(res != "all"){
//try{
//////////////////////
//getinfo
List<IMyEntity> entitys = new List<IMyEntity>();
GridTerminalSystem.GetBlocksOfType<IMyEntity>(entitys);

float totres = 0;
List<IMyInventory> invs = new List<IMyInventory>();
List<float> invcounts = new List<float>();
int invcount = 0;
int whilIA = 0;
while(entitys.Count > whilIA){
IMyEntity entity = entitys[whilIA];
IMyInventory inv = entity.GetInventory();
try{
List<IMyInventoryItem> items = inv.GetItems();
invcount++;
invs.Add(inv);

string org = "";
float numb = 0;
int whilIAC = 0;
while(items.Count > whilIAC){
IMyInventoryItem item = items[whilIAC];
org = ""+item;
numb = float.Parse(org.Substring(0,org.IndexOf("x")));
if(org.IndexOf(res) != -1){
totres = totres+numb;

};
whilIAC++;
};
invcounts.Add(numb);
}
catch{meth.log(consoles,"inventory.info caught");}

whilIA++;
}; //whilIA end
float resEach = totres/invcount;
//getinfo end
//////////////////////
//equalize items
int whilIAI = 0;
while(invs.Count > whilIAI){
float invcunt = invcounts[whilIAI];
while(resEach-transRate > invcunt){
int grtResEach = invcounts.FindIndex(val => val > resEach-transRate);
List<int> itemInIndex = new List<int>();

int whilIAIC = 0;
while(invs[grtResEach].GetItems().Count > whilIAIC){ //get indexes of res
string itm = ""+invs[grtResEach].GetItems()[whilIAIC];
if(itm.IndexOf(res) != -1){
itemInIndex.Add(whilIAIC);
};
whilIAIC++;
};

int whilIAICT = 0;
while(itemInIndex.Count > whilIAICT){
VRage.MyFixedPoint transRateFP = (VRage.MyFixedPoint)transRate;
try{
invs[whilIAI].TransferItemFrom(invs[grtResEach],0,null,false,transRateFP);
invcunt = invcunt + transRate;
}
catch{meth.log(consoles,"caught TransferItemFrom");};
whilIAICT++;
};
}; //resEach > invcunt

whilIAI++;
}; //whilIAI end
//equalize items end
//////////////////////


lcd.WritePublicText("total "+res+": "+Math.Round(totres,2));
lcd.WritePublicText(lcd.GetPublicText()+"\n"+"inventories: "+invcount);
lcd.WritePublicText(lcd.GetPublicText()+"\n"+res+" in each: "+Math.Round(resEach,2));

//}
//catch{meth.log(consoles,"inventory.info caught");}
}; //inventory.info end
/////////////////////////////


}; //info end
/////////////////////////////////////

//inventory end
////////////////////////////////////////////////////////////////////



whil++;
};  //while end

//lcd cycle end
////////////////////////////////////////////////////////////////////////////////
} //void main end
//} //program close
////////////////////////////////////////////////////////////////////////////////////////
public class meths
{
////////////////////////////////////////////////////////////////////////////////
public void log (List<IMyTextPanel> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextPanel console = consoles[whil];
if(clr == true){
console.WritePublicText("");
};
if(nl == true){ //nl true
console.WritePublicText(console.GetPublicText()+"\n");
}; //nl true end
console.WritePublicText(console.GetPublicText()+strng);
whil++;
}; //consoles > whil end

} //log end
////////////////////////////////////////////////////////////////////////////////
}//meths end no } this must be last class
////////////////////////////////////////////////////////////////////////////////////////
