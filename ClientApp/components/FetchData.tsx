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
        setInterval(this.GetStats,10000)

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

    
    private GetStats = () =>{
        ItemService.GetStats()
        .then(response=>{
            if(response.ok){
                (response.json() as Promise<ItemStatistics>)
                .then(data=>{
                    this.setState({stats: data});
                })
            }
            this.setState({loading: false})
        })
        .catch(ex=>{
            this.setState({loading: false})
        })
    }

    private calculateAverage(inputNumber: number){
        return (inputNumber/3)
    }

    private calculateCreatedDeletedRatio(createdValue: number, deletedValue: number){
        if( deletedValue == 0){
            return (this.calculateAverage(createdValue).toFixed(2));
        }

        return (this.calculateAverage(createdValue)/this.calculateAverage(deletedValue)).toFixed(2);
    }

    private getTime(dateStr: string){
        const date = new Date(dateStr);
        const hours = (date.getHours()<10?'0':'') + date.getHours();
        const minutes = (date.getMinutes()<10?'0':'') + date.getMinutes();
        return(`${hours}:${minutes}`)
    }

    private renderStats(){
        if(this.state.stats == null){
            return <h3>No statisitcs found</h3>
        }
        return(
            <div>
                <h3>Trends for the last 3 hours as of {this.getTime(this.state.stats.PolledAt)}</h3>
                <hr/>
                <div>
                    <h4>Average Records Per Hour:</h4>
                    <span>{this.calculateAverage(this.state.stats.ActiveCount).toFixed(2)} Created/Hr</span>
                </div>
                <hr/>
                <div>
                    <h4>Average Records Created/Deleted Per Hour:</h4>
                    <span>{`${this.calculateCreatedDeletedRatio(this.state.stats.ActiveCount,this.state.stats.DeletedCount)} (Created/Hr) / (Deleted/Hr)`}</span>
                </div>
            </div>
        )
    }
}

interface ItemStatistics {
    ActiveCount: number
    DeletedCount: number
    PolledAt: string
}
