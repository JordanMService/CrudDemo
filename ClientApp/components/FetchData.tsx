import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface FetchDataExampleState {
    sampleData: Item[];
    loading: boolean;
}

export class FetchData extends React.Component<RouteComponentProps<{}>, FetchDataExampleState> {
    constructor() {
        super();
        this.state = { sampleData: [], loading: true };

        fetch('api/SampleData/Get')
            .then(response => response.json() as Promise<Item[]>)
            .then(data => {
                this.setState({ sampleData: data, loading: false });
            });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderItemTable(this.state.sampleData);

        return <div>
            <h1>Item Statistics</h1>
            <p>Below are the global and per hour statistics.</p>
         
            { contents }
        </div>;
    }


  

    private static renderItemTable(data: Item[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Phone Number</th>
                </tr>
            </thead>
            <tbody>
            {data.map(item =>
                <tr key={ item.Id }>
                    <td>{ item.Id }</td>
                    <td>{ item.Name }</td>
                    <td>{ item.PhoneNumber}</td>
                </tr>
            )}
            </tbody>
        </table>;
    }
}

interface Item {
    Id: string;
    Name: string;
    PhoneNumber: string;
}
