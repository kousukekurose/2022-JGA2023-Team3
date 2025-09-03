using System;

//レシピデータを格納する構造体
[Serializable]
public struct RecipeData
{
    //ステータス
    public int cookingId;
    public int ingredientId;
}
