using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class StartProgram : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials("ap-northeast-2:135d55dd-d6bd-4f93-80f7-a13ecb711f2f", RegionEndpoint.APNortheast2);

        DBclient = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(DBclient);
        CreateCharacter();
       // FindItem();
    }

    [DynamoDBTable("PlayerInfo")]
    public class Character
    {
        [DynamoDBHashKey] // Hash key.
        public string id { get; set; }
        [DynamoDBProperty]
        public int Info { get; set; }
    }

    private void CreateCharacter() //ĳ���� ������ DB�� �ø���
    {
        Character c1 = new Character
        {
            id = "happy",
            Info = 1111,
        };
        context.SaveAsync(c1, (result) =>
        {
            //id�� happy, Info�� 1111�� ĳ���� ������ DB�� ����
            if (result.Exception == null)
                Debug.Log("Success!");
            else
                Debug.Log(result.Exception);
        });
    }

    public void FindInfo() //DB���� ĳ���� ���� �ޱ�
    {
        Character c;
        context.LoadAsync<Character>("abcd", (AmazonDynamoDBResult<Character> result) =>
        {
            // id�� abcd�� ĳ���� ������ DB���� �޾ƿ�
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }
            c = result.Result;
            Debug.Log(c.Info); //ã�� ĳ���� ���� �� ������ ���� ���
        }, null);
    }
}
