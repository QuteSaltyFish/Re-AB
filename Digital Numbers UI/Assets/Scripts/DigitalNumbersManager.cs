using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalNumbersManager : MonoBehaviour {

    /*
     * Call the public function InputNumber(int inputNumber) to change the number that shown on the UI canvas.
     * The parameter inputNumber, which could either be positive or negative,
     * should be the number which is added to the current number.
     * 
     * Warning:This function can not avoid error from overflow and high frequency changing.
    */

    public List<Sprite> numbersSprites;    //[0]~[9] is number 0 ~ 9
    public List<Image> origNumbersImages;        //[0]~[3] is One to Thousand
   
    [SerializeField]
    private int origNumber = 3000;
    [SerializeField]
    private int changingSpeed = 30;      //the frequency that number changes
    private int targetNumber;

    private void Start()
    {
        UpdateNumbersUI(origNumber);
        targetNumber = origNumber;
    }

    private void LateUpdate()
    {
        //Only a test function which should be deleted later.
        InputTest();
        if(targetNumber!=origNumber){
            ChangeNumber(targetNumber);
        }
    }

    //Update the digital numbers output
    private void UpdateNumbersUI(int currentNumber){
        int crtBit = 0;
        while (currentNumber != 0)
        {
            int tmpNumber = currentNumber % 10;
            currentNumber /= 10;
            origNumbersImages[crtBit].sprite = numbersSprites[tmpNumber];
            crtBit++;
        }
    }

    //When number is gonna be changed
    public void InputNumber(int inputNumber){
        if(inputNumber!=0){
            targetNumber = origNumber + inputNumber;
        }
    }

    private void ChangeNumber(int tNumber){
        if (origNumber < tNumber) {
            origNumber++;
            UpdateNumbersUI(origNumber);
        }
        else{
            origNumber--;
            UpdateNumbersUI(origNumber);
        }
    }

    private void InputTest(){
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InputNumber(-500);
        }
        else if(Input.GetKeyDown(KeyCode.W)){
            InputNumber(60);
        }
        else if(Input.GetKeyDown(KeyCode.E)){
            InputNumber(15);
        }
        else if(Input.GetKeyDown(KeyCode.R)){
            InputNumber(8);
        }
    }
}
