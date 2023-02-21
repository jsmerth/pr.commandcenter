import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.less']
})

export class FetchDataComponent {

  private http: HttpClient;

  public bots: IBot[] = [];
  private _bots: IBot[] = [];

  public detailIsActive: boolean = false;
  public detailBot?: IBot;
  public detailEvents: IEvent[] = [];

  public filters: IFilter[] = [];
  public suggestedFilters: IFilter[] = [];
  public filterKeyword: string = "";
  public filterIsActive: boolean = false;
  public countries: IFilter[] = [];
  public regions: IFilter[] = [];
  public cities: IFilter[] = [];
  public businesses: IFilter[] = [];
  public statuses: IFilter[] = [];
  public errors: number = 0;

  public mapIsActive: boolean = true;
  public map: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.http = http;
    this.http.get<IBots>('http://localhost:7234/api/Bots').subscribe(result => {
      //set master list
      this._bots = result.items;
      //set working list
      this.bots = result.items;

      this.errors = this.bots.filter((v: IBot) => v.status === 'ERROR').length;

      this._buildFilterableItems();

    }, error => console.error(error));
  }

  public clickMarker = (b: IBot) => {
    this.clickBotId(b);
  }
  public clickBotId = (bot: IBot) => {

    this.http.get<IEvents>('http://localhost:7234/api/Events/' + bot.id).subscribe(result => {
      this.detailIsActive = true;
      this.detailBot = bot;
      this.detailEvents = result.items;

    }, error => console.error(error));
  }
  //- adds a new filter if it isn't a duplicate
  public clickBotProperty = (property: string, value: string) => {
    if (!this.filters.some(f => f.property === property && f.value === value)) {
      this.filters.push({ property, value })

      this._filter();
    }
    this.suggestedFilters = [];
    this.filterKeyword = "";
  }
  public clickDetailClose = () => {
    this.detailIsActive = false;
    this.detailBot = undefined;
  }
  //- removes an existing filter
  public clickFilterProperty = (property: string, value: string) => {
    this.filters = this.filters.filter((v, i, a) => {
      if (v.property === property && v.value === value) return false;
      return true;
    });

    this._filter();
  }
  //- clears the filters and resets the bot list to the master list
  public clearFilter() {
    this.bots = this._bots;
    this.filters = [];
    this._buildFilterableItems();
  }
  public keypressFilter = (keyword: string) => {
    const regex = new RegExp(keyword, 'gi');
    this.suggestedFilters = [...this.countries, ...this.regions, ...this.cities, ...this.businesses, ...this.statuses].filter((v, i, a) => v.value.match(regex));
  }
  public blurFilter = () => {
    setTimeout(() => {
      this.filterIsActive = false;
      this.suggestedFilters = [];
    }, 500)
  }
  public focusFilter = () => {
    this.filterIsActive = true;
  }

  private _filter = () => {
    this.bots = this._bots.filter((v, i, a) => {
      let include = true;
      this.filters.forEach(x => {
        if (v[x.property as keyof IBot] !== x.value) include = false;
      });
      return include;
    });

    this._buildFilterableItems();
  }
  private _buildFilterableItems = () => {
    this.countries = [...new Set(this.bots.map((v) => v.country).sort())].map((v) => new Filter('country', v));
    this.regions = [...new Set(this.bots.map((v) => v.region).sort())].map((v) => new Filter('region', v));
    this.cities = [...new Set(this.bots.map((v) => v.city).sort())].map((v) => new Filter('city', v));
    this.businesses = [...new Set(this.bots.map((v) => v.business).sort())].map((v) => new Filter('business', v));
    this.statuses = [...new Set(this.bots.map((v) => v.status).sort())].map((v) => new Filter('status', v));
  }
}

interface IBots {
  items: IBot[]
}
interface IBot {
  id: string,
  type: string,
  business: string,
  city: string,
  region: string,
  country: string,
  lat: number,
  lon: number,
  status: string,
  dateModified: Date,
  dateCreated: Date,
}
interface IEvents {
  items: IEvent[]
}
interface IEvent {
  id: string,
  botId: string,
  type: string,
  content: string,
  dateCreated: Date
}
interface IFilter {
  property: string,
  value: string
}
class Filter implements IFilter {
  constructor(p: string, v: string) {
    this.value = v;
    this.property = p;
  }
  value: string = '';
  property: string = '';
}
