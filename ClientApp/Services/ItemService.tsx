export default class ItemService{

    public static PostData(name: string, phoneNumber: string) : Promise<Response>{
       return fetch("/api/SampleData/Post",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(
                {
                    Id: "",
                    Name: name,
                    PhoneNumber: phoneNumber
                }as Item)
        })
    }

    public static DeleteItem(itemId: string) : Promise<Response>{
        return fetch(`api/SampleData/Delete/${itemId}`)
    }

    public static GetItem(itemId: string) : Promise<Response>{
        return fetch(`api/SampleData/Get/${itemId}`)
    }

    public static GetStats(): Promise<Response>{
        return fetch("api/SampleData/Stats")
    }
}

export interface Item {
    Id: string;
    Name: string;
    PhoneNumber: string;
}
