import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import ItemService from "../Services/ItemService";
import 'isomorphic-fetch';

interface FetchDataExampleState {
    stats: ItemStatistics | null;
    loading: boolean;
}

export class FetchData extends React.Component<RouteComponentProps<{}>, FetchDataExampleState> {
    constructor() {
        super();
        this.state = { stats: null, loading: true };

        this.GetStats();

    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderStats();

        return <div>
            <h1>Item Statistics</h1>
            <p>Below are the global and per hour statistics.</p>
         
            { contents }
        </div>;
    }

    //Todo make this repeat
    private GetStats(){
        ItemService.GetStats()
        .then(response=>{
            if(response.ok){
                (response.json() as Promise<ItemStatistics>)
                .then(data=>{
                    debugger;
                    this.setState({stats: data});
                })
            }
            this.setState({loading: false})
        })
        .catch(ex=>{
            this.setState({loading: false})
        })
    }

    private renderStats(){
        if(this.state.stats == null){
            return <h3>No statisitcs found</h3>
        }
        return(
            <div>
                <h3>Trends for the last 3 hours</h3>
                <div>
                    <h4>Average Records Per Hour:</h4>
                    <span>{this.state.stats.ActiveCount/3}</span>
                </div>
                <div>
                    <h4>Average Records Created/Deleted Per Hour:</h4>
                    <span>{`${(this.state.stats.ActiveCount/3)}:${(this.state.stats.DeletedCount/3)}`}</span>
                </div>
            </div>
        )
    }
}

interface ItemStatistics {
    ActiveCount: number
    DeletedCount: number
}
