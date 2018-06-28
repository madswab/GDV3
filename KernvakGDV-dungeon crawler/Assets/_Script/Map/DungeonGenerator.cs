using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public int seed;

    public int mapX = 20; //row
    public int mapY = 10; //columns
    public List<Vector2> rooms = new List<Vector2>();
    public int attempts = 100;
    public char[,] dungeon;

    public string characters;
    private string[] charactersUpFriends;
    private string[] charactersDownFriends;
    private string[] charactersLeftFriends;
    private string[] charactersRightFriends;


    void Awake() {
        InitializeCharacters();
        //InitializeDungeon();
       // DisplayDungeon();
    }

    void Update() {

    }


    public void InitializeDungeon() {
        dungeon = new char[mapX, mapY];

        InitializeWalls();

        for (int y = 1; y < mapY - 1; y++) {
            for (int x = 1; x < mapX - 1; x++) {
                dungeon[x, y] = 'O';
            }
        }

        Random.InitState(seed);

        CreateRooms();

        for (int y = 1; y < mapY - 1; y++) {
            for (int x = 1; x < mapX - 1; x++) {

                if (dungeon[x, y] == '@' || dungeon[x, y] == '˄' || dungeon[x, y] == '˂' || dungeon[x, y] == '˃' || dungeon[x, y] == '˅' || 
                    dungeon[x, y] == '╔' || dungeon[x, y] == '╧' || dungeon[x, y] == '╤' || dungeon[x, y] == '╗' || dungeon[x, y] == '╟' || 
                    dungeon[x, y] == '╢' || dungeon[x, y] == '╚' || dungeon[x, y] == '╝')
                {
                    continue;
                }

                string valid = GetValidCharacters(x, y);
                dungeon[x, y] = valid[Random.Range(0, valid.Length)];
            }
        }

        DeadEnds();
    }

    private void CreateRooms(){
        for (int i = 0; i < rooms.Count; i++){
            Vector2 startPosRoom = CheckRoomSpace(i);
            if (startPosRoom == Vector2.zero){
                continue;
            }

            for (int y = (int)Mathf.Floor(startPosRoom.y); y < (int)Mathf.Floor(startPosRoom.y) + rooms[i].y; y++){
                for (int x = (int)Mathf.Floor(startPosRoom.x); x < (int)Mathf.Floor(startPosRoom.x) + rooms[i].x; x++){
                    
                    //hoeken
                    if (x == (int)Mathf.Floor(startPosRoom.x) && y == (int)Mathf.Floor(startPosRoom.y)){
                        dungeon[x, y] = '╔';
                        continue;
                    }
                    if (x == (int)Mathf.Floor(startPosRoom.x) + rooms[i].x - 1 && y == (int)Mathf.Floor(startPosRoom.y)){
                        dungeon[x, y] = '╗';
                        continue;
                    }
                    if (x == (int)Mathf.Floor(startPosRoom.x) && y == (int)Mathf.Floor(startPosRoom.y) + rooms[i].y - 1){
                        dungeon[x, y] = '╚';
                        continue;
                    }
                    if (x == (int)Mathf.Floor(startPosRoom.x) + rooms[i].x - 1 && y == (int)Mathf.Floor(startPosRoom.y) + rooms[i].y - 1){
                        dungeon[x, y] = '╝';
                        continue;
                    }

                    //ingang
                    if (x == (int)Mathf.Floor(startPosRoom.x) && y == (int)Mathf.Floor(startPosRoom.y) + (int)Mathf.Floor((rooms[i].y) * 0.5f)){
                        dungeon[x, y] = '˂';
                        continue;
                    }
                    if (x == (int)Mathf.Floor(startPosRoom.x) + (int)Mathf.Floor(rooms[i].x) - 1 && y == (int)Mathf.Floor(startPosRoom.y) + (int)Mathf.Floor((rooms[i].y) * 0.5f)){
                        dungeon[x, y] = '˃';
                        continue;
                    }

                    //walls
                    if (x > (int)Mathf.Floor(startPosRoom.x) && x < (int)Mathf.Floor(startPosRoom.x) + (int)Mathf.Floor(rooms[i].x) - 1 && y == (int)Mathf.Floor(startPosRoom.y)){
                        dungeon[x, y] = '╤';
                        continue;
                    }
                    if (x > (int)Mathf.Floor(startPosRoom.x) && x < (int)Mathf.Floor(startPosRoom.x) + (int)Mathf.Floor(rooms[i].x) - 1 && y == (int)Mathf.Floor(startPosRoom.y) + (int)Mathf.Floor(rooms[i].y) - 1){
                        dungeon[x, y] = '╧';
                        continue;
                    }
                    if (y > (int)Mathf.Floor(startPosRoom.y) && y < (int)Mathf.Floor(startPosRoom.y) + (int)Mathf.Floor(rooms[i].y) - 1 && x == (int)Mathf.Floor(startPosRoom.x)){
                        dungeon[x, y] = '╟';
                        continue;
                    }
                    if (y > (int)Mathf.Floor(startPosRoom.y) && y < (int)Mathf.Floor(startPosRoom.y) + (int)Mathf.Floor(rooms[i].y) - 1 && x == (int)Mathf.Floor(startPosRoom.x) + (int)Mathf.Floor(rooms[i].x) - 1){
                        dungeon[x, y] = '╢';
                        continue;
                    }

                    //rest
                    dungeon[x, y] = '@';
                }
            }
#region
            /*
            int startRow = Random.Range(2, mapX - 7);
            int startColumn = Random.Range(2, mapY - 7);

            //˂˃˄˅
            //╔╗╚╝╟╢╤╧
            dungeon[startRow, startColumn] = '╔';
            dungeon[startRow + 1, startColumn] = '╤';
            dungeon[startRow + 2, startColumn] = '╤';
            dungeon[startRow + 3, startColumn] = '╤';
            dungeon[startRow + 4, startColumn] = '╤';
            dungeon[startRow + 5, startColumn] = '╗';

            dungeon[startRow, startColumn + 1] = '╟';
            dungeon[startRow + 1, startColumn + 1] = '@';
            dungeon[startRow + 2, startColumn + 1] = '@';
            dungeon[startRow + 3, startColumn + 1] = '@';
            dungeon[startRow + 4, startColumn + 1] = '@';
            dungeon[startRow + 5, startColumn + 1] = '╢';

            dungeon[startRow, startColumn + 2] = '˂';
            dungeon[startRow + 1, startColumn + 2] = '@';
            dungeon[startRow + 2, startColumn + 2] = '@';
            dungeon[startRow + 3, startColumn + 2] = '@';
            dungeon[startRow + 4, startColumn + 2] = '@';
            dungeon[startRow + 5, startColumn + 2] = '˃';

            dungeon[startRow, startColumn + 3] = '╟';
            dungeon[startRow + 1, startColumn + 3] = '@';
            dungeon[startRow + 2, startColumn + 3] = '@';
            dungeon[startRow + 3, startColumn + 3] = '@';
            dungeon[startRow + 4, startColumn + 3] = '@';
            dungeon[startRow + 5, startColumn + 3] = '╢';

            dungeon[startRow, startColumn + 4] = '╚';
            dungeon[startRow + 1, startColumn + 4] = '╧';
            dungeon[startRow + 2, startColumn + 4] = '╧';
            dungeon[startRow + 3, startColumn + 4] = '╧';
            dungeon[startRow + 4, startColumn + 4] = '╧';
            dungeon[startRow + 5, startColumn + 4] = '╝';*/
#endregion
        }     
    }

    private Vector2 CheckRoomSpace(int room){
        Vector2 startPos = Vector2.zero;
        int att = attempts;

        while (att > 0 && startPos == Vector2.zero){
            int startX = Random.Range(2, mapX - (int)Mathf.Floor(rooms[room].x) - 2);
            int startY = Random.Range(2, mapY - (int)Mathf.Floor(rooms[room].y) - 2);

            if (CheckNeighbor(new Vector2(startX, startY), rooms[room])){
                startPos = new Vector2(startX, startY);
            }
            att--;
        }

        if (att == 0 && startPos == Vector2.zero){
            return Vector2.zero;
        }

        return startPos;
    }

    private bool CheckNeighbor(Vector2 pos, Vector2 size){
        bool fine = true;

        for (int y = (int)Mathf.Floor(pos.y); y < (int)Mathf.Floor(pos.y) + size.y; y++){
            if (dungeon[(int)Mathf.Floor(pos.x) + 2, y] != 'O' || dungeon[(int)Mathf.Floor(pos.x) - 2, y] != 'O' ||
                dungeon[(int)Mathf.Floor(pos.x) + (int)Mathf.Floor(size.x) + 2, y] != 'O' || dungeon[(int)Mathf.Floor(pos.x) + (int)Mathf.Floor(size.x) - 2, y] != 'O')
            {
                fine = false;
            }
        }

        for (int x = (int)Mathf.Floor(pos.x); x < (int)Mathf.Floor(pos.x) + size.x; x++){
            if (dungeon[x, (int)Mathf.Floor(pos.y) + 1] != 'O' || dungeon[x, (int)Mathf.Floor(pos.y) - 1] != 'O' ||
                dungeon[x, (int)Mathf.Floor(pos.y) + (int)Mathf.Floor(size.y) + 1] != 'O' || dungeon[x, (int)Mathf.Floor(pos.y) + (int)Mathf.Floor(size.y) - 1] != 'O')
            {
                fine = false;
            }
        }

        return fine;
    }

    private void DeadEnds()  {
        //►◄▲▼
        for (int y = 1; y < mapY - 1; y++){
            for (int x = 1; x < mapX - 1; x++){
                if (dungeon[x, y] == 'O'){
                    if ("─┐┘┤┬┴┼˂".Contains(dungeon[x + 1, y].ToString())){
                        dungeon[x, y] = '►';
                    }
                    else if ("─┌└├┬┴┼˃".Contains(dungeon[x - 1, y].ToString())){
                        dungeon[x, y] = '◄';
                    }
                    else if ("│┌┐├┤┬┼˅".Contains(dungeon[x, y - 1].ToString())){
                        dungeon[x, y] = '▲';
                    }
                    else if ("│└┘├┤┴┼˄".Contains(dungeon[x, y + 1].ToString())){
                        dungeon[x, y] = '▼';
                    }
                    else{
                        //Debug.Log("O");
                    }
                }
            }
        }
    }

    private void InitializeWalls() {
        for (int y = 0; y < mapY; y++) {
            dungeon[0, y] = 'X';
            dungeon[mapX - 1, y] = 'X';
        }

        for (int x = 0; x < mapX; x++) {
            dungeon[x, 0] = 'X';
            dungeon[x, mapY - 1] = 'X';
        }
    }

    private string GetValidCharacters(int x, int y) {
        string valid = "";

        for (int i = 0; i < characters.Length; i++) {
            char c = characters[i];

            if (charactersLeftFriends[i].Contains(dungeon[x - 1, y].ToString()) && charactersRightFriends[i].Contains(dungeon[x + 1, y].ToString()) &&
                charactersUpFriends[i].Contains(dungeon[x, y - 1].ToString()) && charactersDownFriends[i].Contains(dungeon[x, y + 1].ToString()))
            {
                valid += c.ToString();
            }
        }

        if (valid.Length == 0) {
            valid = "O";
        }

        return valid;
    }

    public void DisplayDungeon() {
        string output = "";

        for (int y = 0; y < mapY; y++){
            for (int x = 0; x < mapX; x++){
                output += dungeon[x, y];
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    public bool[,] LoopDungeon() {
        bool[,] visitedCells = new bool[mapX, mapY];
        int currentX = 1;
        int currentY = 1;

        LoopCells(visitedCells, currentX, currentY);

        int amountCellsTrue = 0;
        foreach (var item in visitedCells){
            if (item){
                amountCellsTrue++;
            }
        }

        return visitedCells;
    }

    private void LoopCells(bool[,] visitedCells, int currentX, int currentY){
        if (visitedCells [currentX, currentY]){
            return;
        }

        visitedCells[currentX, currentY] = true;

        switch (dungeon[currentX, currentY]){
            case '┌':
            case '╔':
                LoopCells(visitedCells, currentX, currentY + 1);
                LoopCells(visitedCells, currentX + 1, currentY);
                break;
            case '┐':
            case '╗':
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX, currentY + 1);
                break;
            case '─':
            case '˂':
            case '˃':
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX + 1, currentY);
                break;
            case '│':
            case '˄':
            case '˅':
                LoopCells(visitedCells, currentX, currentY - 1);
                LoopCells(visitedCells, currentX, currentY + 1);
                break;
            case '└':
            case '╚':
                LoopCells(visitedCells, currentX, currentY - 1);
                LoopCells(visitedCells, currentX + 1, currentY);
                break;
            case '┘':
            case '╝':
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX, currentY - 1);
                break;
            case '├':
            case '╟':
                LoopCells(visitedCells, currentX + 1, currentY);
                LoopCells(visitedCells, currentX, currentY - 1);
                LoopCells(visitedCells, currentX, currentY + 1);
                break;
            case '┤':
            case '╢':
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX, currentY + 1);
                LoopCells(visitedCells, currentX, currentY - 1);
                break;
            case '┬':
            case '╤':
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX, currentY + 1);
                LoopCells(visitedCells, currentX + 1, currentY);
                break;
            case '┴':
            case '╧':
                LoopCells(visitedCells, currentX, currentY - 1);
                LoopCells(visitedCells, currentX + 1, currentY);
                LoopCells(visitedCells, currentX - 1, currentY);
                break;
            case '┼':
            case '@':
                LoopCells(visitedCells, currentX, currentY - 1);
                LoopCells(visitedCells, currentX, currentY + 1);
                LoopCells(visitedCells, currentX - 1, currentY);
                LoopCells(visitedCells, currentX + 1, currentY);
                break;
            case 'O':
            case '►':
            case '◄':
            case '▲':
            case '▼':               
                return; // This is one of those pesky dead-ends!
            default:
                //
                return;
        }
    }

    private void InitializeCharacters(){
        characters = "─│┌┐└┘├┤┬┴┼";

        charactersUpFriends = new string[characters.Length];
        charactersDownFriends = new string[characters.Length];
        charactersLeftFriends = new string[characters.Length];
        charactersRightFriends = new string[characters.Length];

        //˂˃˄˅
        //╔╗╚╝╟╢╤╧
        charactersLeftFriends[0] = "O─┌└├┬┴┼";              //─
        charactersLeftFriends[1] = "O│┐┘┤X╟╢╤╧╔╗╚╝";        //│
        charactersLeftFriends[2] = "O│┐┘┤X╟╢╤╧╔╗╚╝";        //┌
        charactersLeftFriends[3] = "O─┌└├┬┴┼";              //┐
        charactersLeftFriends[4] = "O│┐┘┤X╟╢╤╧╔╗╚╝";        //└
        charactersLeftFriends[5] = "O─┌└├┬┴┼";              //┘
        charactersLeftFriends[6] = "O│┐┘┤X╟╢╤╧╔╗╚╝";        //├
        charactersLeftFriends[7] = "O─┌└├┬┴┼˃";             //┤
        charactersLeftFriends[8] = "O─┌└├┬┴┼˃";             //┬
        charactersLeftFriends[9] = "O─┌└├┬┴┼˃";             //┴
        charactersLeftFriends[10] = "O─┌└├┬┴┼˃";            //┼
                                                          
        charactersRightFriends[0] = "O─┐┘┤┬┴┼";             //─
        charactersRightFriends[1] = "O│┌└├X╟╢╤╧╔╗╚╝";       //│
        charactersRightFriends[2] = "O─┐┘┤┬┴┼˂";            //┌
        charactersRightFriends[3] = "O│┌└├X╟╢╤╧╔╗╚╝";       //┐
        charactersRightFriends[4] = "O─┐┘┤┬┴┼";             //└
        charactersRightFriends[5] = "O│┌└├X╟╢╤╧╔╗╚╝";       //┘
        charactersRightFriends[6] = "O─┐┘┤┬┴┼˂";            //├
        charactersRightFriends[7] = "O│┌└├X╟╢╤╧╔╗╚╝";       //┤
        charactersRightFriends[8] = "O─┐┘┤┬┴┼˂";            //┬
        charactersRightFriends[9] = "O─┐┘┤┬┴┼˂";            //┴
        charactersRightFriends[10] = "O─┐┘┤┬┴┼˂";           //┼

        charactersUpFriends[0] = "O─└┘┴X╟╢╤╧╔╗╚╝";          //─
        charactersUpFriends[1] = "O│┌┐├┤┬┼";                //│
        charactersUpFriends[2] = "O─└┘┴X╟╢╤╧╔╗╚╝";          //┌
        charactersUpFriends[3] = "O─└┘┴X╟╢╤╧╔╗╚╝";          //┐
        charactersUpFriends[4] = "O│┌┐├┤┬┼";                //└
        charactersUpFriends[5] = "O│┌┐├┤┬┼";                //┘
        charactersUpFriends[6] = "O│┌┐├┤┬┼˅";               //├
        charactersUpFriends[7] = "O│┌┐├┤┬┼˅";               //┤
        charactersUpFriends[8] = "O─└┘┴X╟╢╤╧╔╗╚╝";          //┬
        charactersUpFriends[9] = "O│┌┐├┤┬┼˅";               //┴
        charactersUpFriends[10] = "O│┌┐├┤┬┼˅";              //┼

        charactersDownFriends[0] = "O─┌┐┬X╟╢╤╧╔╗╚╝";        //─
        charactersDownFriends[1] = "O│└┘├┤┴┼";              //│
        charactersDownFriends[2] = "O│└┘├┤┴┼";              //┌
        charactersDownFriends[3] = "O│└┘├┤┴┼";              //┐
        charactersDownFriends[4] = "O─┌┐┬X╟╢╤╧╔╗╚╝";        //└
        charactersDownFriends[5] = "O─┌┐┬X╟╢╤╧╔╗╚╝";        //┘
        charactersDownFriends[6] = "O│└┘├┤┴┼˄";             //├
        charactersDownFriends[7] = "O│└┘├┤┴┼˄";             //┤
        charactersDownFriends[8] = "O│└┘├┤┴┼˄";             //┬
        charactersDownFriends[9] = "O─┌┐┬X╟╢╤╧╔╗╚╝";        //┴
        charactersDownFriends[10] = "O│└┘├┤┴┼˄";            //┼
    }

}
