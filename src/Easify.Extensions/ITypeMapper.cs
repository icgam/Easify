// This software is part of the Easify framework
// Copyright (C) 2019 Intermediate Capital Group
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

 namespace EasyApi.Extensions
{
    // TODO: Should have a base implementation of this as we do in many different applications
    public interface ITypeMapper
    {
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination); // TODO: They are redundant with no usage.
        TDestination Map<TSource, TDestination>(TSource source); // TODO: They are redundant with no usage.
        TDestination Map<TDestination>(object source);
    }
}
