import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "orderByDate"
})
export class OrderByPipe implements PipeTransform {
  transform(array: any, field: string): any[] {
    array.sort((a: any, b: any) => {
      return new Date(b[field]).getTime() - new Date(a[field]).getTime();
    });
    return array;
  }
}